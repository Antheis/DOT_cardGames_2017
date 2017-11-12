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

            dataSerializer = DPSManager.GetDataSerializer<BinaryFormaterSerializer>();
            dataProcessors = new List<DataProcessor>();
            dataProcessorOptions = new Dictionary<string, string>();
        }

        public void start()
        {
            Connection TCPconn = TCPConnection.GetConnection(new ConnectionInfo(IP, Port));
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);
            //NetworkComms.AppendGlobalIncomingPacketHandler<byte[]>("ArrayByte", todo);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);

            TCPconn.AppendShutdownHandler(disconnect);
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);
        }

        private void disconnect(Connection conn)
        {

        }
    }
}
