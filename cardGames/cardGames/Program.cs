using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using System.Runtime.InteropServices;


namespace cardGame_Client
{
    class Program
    {
        private static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int evt);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            Client client = new Client();
            client.start();
        }

        static bool ConsoleEventCallback(int evt)
        {
            Console.WriteLine("Closing NetworkComms...");
            NetworkComms.Shutdown();
            return false;
        }

    /*static void Main(string[] args)
    {
        DataSerializer dataSerializer = DPSManager.GetDataSerializer<BinaryFormaterSerializer>();
        List<DataProcessor> dataProcessors = new List<DataProcessor>();
        Dictionary<string, string> dataProcessorOptions = new Dictionary<string, string>();

        NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);

        var slaveId = Process.GetCurrentProcess().Id;
        var endpoint = GetIPEndPointFromHostName("localhost", 4242);
        var connection = TCPConnection.GetConnection(new ConnectionInfo(endpoint));
        var slaveName = string.Format("Slave{0}", slaveId);

        connection.SendObject("Message", string.Format("{0} reporting for duty!", slaveName));

    }

    private static IPEndPoint GetIPEndPointFromHostName(string hostName, int port)
    {
        var addresses = Dns.GetHostAddresses(hostName);
        if (addresses.Length == 0)
        {
            throw new ArgumentException(
                "Unable to retrieve address from specified host name.",
                "hostName"
            );
        }
        return new IPEndPoint(addresses[0], port);
    }*/
}
}
