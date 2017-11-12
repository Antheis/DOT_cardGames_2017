using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using NetworkCommsDotNet.Connections.TCP;
using Protocol;

namespace cardGame_Client
{
    class Client
    {
        private DataSerializer dataSerializer { get; set; }
        private List<DataProcessor> dataProcessors { get; set; }
        private Dictionary<string, string> dataProcessorOptions { get; set; }
        private string IP;
        private int Port;
        private string ID;
        //private List<string> Bataille_available_actions = new List<string>(new string[] { });
        //private List<string> BJ_available_actions = new List<string>(new string[] { });
        private List<string> menu_available_actions = new List<string>(new string[] { "1 -'BJ' to play blackjack", "2 -'BT' to play bataille", "3 -'help' to get available commands", "4 -'quit' to close the client" });
        private enum State { BlackJack, Bataille, Menu };

        public Client()
        {
            /*string src;

            Console.WriteLine("Please enter the server IP and port in the format IP:Port");
            src = Console.ReadLine();
            IP = src.Split(':').First();
            Port = int.Parse(src.Split(':').Last());

            Console.WriteLine("Please give me your ID");
            ID = Console.ReadLine();
            ID += ":" + Process.GetCurrentProcess().Id;

            dataSerializer = DPSManager.GetDataSerializer<ProtobufSerializer>();
            dataProcessors = new List<DataProcessor>();
            dataProcessorOptions = new Dictionary<string, string>();*/
        }

        private void Bataille()
        {
            Console.WriteLine("Bataille");
            Console.WriteLine("\n");
        }

        private void BlackJack()
        {
            Console.WriteLine("BlackJack");
            Console.WriteLine("\n");
        }

        private void printhelp()
        {
            foreach (string T in menu_available_actions) { Console.WriteLine(T); }
            Console.WriteLine("\n");
        }

        public void start()
        {
            /*Connection TCPconn = TCPConnection.GetConnection(new ConnectionInfo(IP, Port));
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            NetworkComms.AppendGlobalIncomingPacketHandler<ProtocolCl>("Protocol", PrintIncomingMessage);
            TCPconn.AppendShutdownHandler(disconnect);*/

            Console.WriteLine("Write 'quit' to quit | Write 'help' to get available commands");
            while (true)
            {
                string line = Console.ReadLine();

                if (line == "quit")
                    break;
                switch (line)
                {
                    case "help":
                        printhelp();
                        break;
                    case "BJ":
                        BlackJack();
                        break;
                    case "BT":
                        Bataille();
                        break;
                }
                //ProtocolCl cmd = new ProtocolCl(Cmd.Ready, Card.None);
                //TCPconn.SendObject("Protocol", cmd);
            }
            //TCPconn.AppendShutdownHandler(disconnect);
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, ProtocolCl message)
        {
            Console.WriteLine("Cmd = " + message.Command);
        }

        private void disconnect(Connection conn)
        {

        }
    }
}
