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
        private string  IP;
        private int     Port;
        private string  ID;
        //private List<string> Bataille_available_actions = new List<string>(new string[] { });
        //private List<string> BJ_available_actions = new List<string>(new string[] { });
        private List<string> menu_available_actions = new List<string>(new string[] { "'BJ' to play blackjack", "'Bataille' to play bataille", "'help' to get available commands", "'quit' to close the client" });

        public Client()
        {
            string src;

            Console.WriteLine("Please enter the server IP and port in the format IP:Port");
            src = Console.ReadLine();
            IP = src.Split(':').First();
            Port = int.Parse(src.Split(':').Last());

            Console.WriteLine("Please give me your ID");
            ID = Console.ReadLine();
            ID += ":" + Process.GetCurrentProcess().Id;

            dataSerializer = DPSManager.GetDataSerializer<ProtobufSerializer>();
            dataProcessors = new List<DataProcessor>();
            dataProcessorOptions = new Dictionary<string, string>();
        }

        public void start()
        {
            Connection TCPconn = TCPConnection.GetConnection(new ConnectionInfo(IP, Port));
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            NetworkComms.AppendGlobalIncomingPacketHandler<ProtocolCl>("Protocol", PrintIncomingMessage);

            Console.WriteLine("Write 'quit' to quit");
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "quit")
                    break;
                ProtocolCl cmd = new ProtocolCl(Cmd.Ready, Card.None);
                TCPconn.SendObject("Protocol", cmd);
            }

            TCPconn.AppendShutdownHandler(disconnect);
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
