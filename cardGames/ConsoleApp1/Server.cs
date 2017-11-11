using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using NetworkCommsDotNet;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;
using NetworkCommsDotNet.Connections.TCP;

namespace cardGame_Server
{
    class Server
    {
        private DataSerializer dataSerializer { get; set; }
        private List<DataProcessor> dataProcessors { get; set; }
        private Dictionary<string, string> dataProcessorOptions { get; set; }

        private int nbGames = 0;
        private int nbClients = 0;
        private List<Game> games = new List<Game>();

        private void addConnection(Connection connection)
        {
            foreach (Game game in games)
            {

            }
            games.Add(new Game());
        }

        public Server()
        {
            dataSerializer = DPSManager.GetDataSerializer<BinaryFormaterSerializer>();
            dataProcessors = new List<DataProcessor>();
            dataProcessorOptions = new Dictionary<string, string>();
        }

        public void Start()
        {
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);

            NetworkComms.AppendGlobalIncomingPacketHandler<byte[]>("ArrayByte", PrintIncomingMessage);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            NetworkComms.AppendGlobalConnectionEstablishHandler(OnConnectionEstablished);
            NetworkComms.AppendGlobalConnectionCloseHandler(OnConnectionClosed);

            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 4242));

            Console.WriteLine("Listening for TCP messages on:");
            foreach (IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            Console.WriteLine("\nPress a key to quit.");
            Console.ReadKey(true);

            NetworkComms.Shutdown();
        }

        private void OnConnectionClosed(Connection connection)
        {
            Console.WriteLine("Connection closed");
        }

        private void OnConnectionEstablished(Connection connection)
        {
            Console.WriteLine("Connection established with " + connection.ToString());
            addConnection(connection);
            //connection.SendObject("Card", Card.Two);
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, byte[] message)
        {
            Console.WriteLine("\nReceived byte array from " + connection.ToString());

            for (int i = 0; i < message.Length; i++)
                Console.WriteLine(i.ToString() + " - " + message[i].ToString());
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("\nReceived string from " + connection.ToString());
            Console.WriteLine(message);
        }
    }
}
