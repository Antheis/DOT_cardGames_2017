using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;
using NetworkCommsDotNet.Connections.TCP;
using Protocol;

namespace cardGame_Server
{
    class Server
    {
        private DataSerializer dataSerializer { get; set; }
        private List<DataProcessor> dataProcessors { get; set; }
        private Dictionary<string, string> dataProcessorOptions { get; set; }

        private List<Game> games = new List<Game>();
        private List<Client> players = new List<Client>();

        private void AddToGame(Client cl)
        {
            lock (games)
            {
                for (int i = 0; i < games.Count; ++i)
                {
                    Console.WriteLine("Checking game...");
                    if (!games[i].IsFull())
                    {
                        cl.setGame(i);
                        games[i].AddClient(cl);
                        lock (players)
                        {
                            players.Add(cl);
                        }
                        return;
                    }
                }
                Console.WriteLine("Creating new Game");
                games.Add(new Game(games.Count));
                cl.setGame(games.Count - 1);
                games[games.Count - 1].AddClient(cl);
                lock (players)
                {
                    players.Add(cl);
                }
            }
        }

        public Server()
        {
            dataSerializer = DPSManager.GetDataSerializer<ProtobufSerializer>();
            dataProcessors = new List<DataProcessor>();
            dataProcessorOptions = new Dictionary<string, string>();
        }

        public void Start()
        {
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);

            NetworkComms.AppendGlobalIncomingPacketHandler<ProtocolCl>("Protocol", PrintIncomingMessage);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            NetworkComms.AppendGlobalConnectionEstablishHandler(OnConnectionEstablished);
            NetworkComms.AppendGlobalConnectionCloseHandler(OnConnectionClosed);

            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 4242));

            Console.WriteLine("Listening for TCP messages on:");
            foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            Console.WriteLine("\nWaiting for games to be launched...");
            while (true)
            {
                lock (games)
                {
                    foreach (Game game in games)
                    {
                        if (game.IsFull() && !game.IsRunning())
                            game.BeginGame();
                        if (game.IsRunning() && !game.IsPlaying())
                            game.PrepareGame();
                        if (game.IsPlaying())
                            game.DoTurn();
                    }
                }
            }
        }

        private void OnConnectionClosed(Connection connection)
        {
            Console.WriteLine("Connection closed");
            lock (players)
            {
                foreach (Client cl in players)
                {
                    if (cl.IsEqual(connection))
                    {
                        if (cl.Game() != -1)
                            lock (games)
                            {
                                if (games[cl.Game()].RemoveClient(connection) && games[cl.Game()].nbPlayers() == 0)
                                {
                                    games.RemoveAt(cl.Game());
                                    break;
                                }
                            }
                        players.Remove(cl);
                        break;
                    }
                }
            }
        }

        private void OnConnectionEstablished(Connection connection)
        {
            Console.WriteLine("Connection established with " + connection.ToString());
            Client cl = new Client(connection);
            AddToGame(cl);
        }

        private void PrintIncomingMessage(PacketHeader header, Connection connection, ProtocolCl message)
        {
            Console.WriteLine("\nReceived protocol from " + connection.ToString());
            Console.WriteLine("Command - " + message.Command);
            switch (message.Command)
            {
                case (Cmd.Ready):
                    foreach (Client cl in players)
                    {
                        if (cl.IsEqual(connection))
                        {
                            cl.setReadyState(true);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("\nReceived string from " + connection.ToString());
            Console.WriteLine(message);
        }
    }
}
