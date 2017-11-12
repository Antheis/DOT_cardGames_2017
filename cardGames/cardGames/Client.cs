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
        private List<string> Bataille_available_actions = new List<string>(new string[] { "1 -'hand' to get your hand", "2 -'Rdy' to launch a new turn"});
        private List<string> BJ_available_actions = new List<string>(new string[] { });
        private List<string> menu_available_actions = new List<string>(new string[] { "1 -'BJ' to play blackjack", "2 -'BT' to play bataille", "3 -'help' to get available commands", "4 -'quit' to close the client" });
        private enum Status { BlackJack, Bataille, Menu };
        Connection TCPconn;

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

        private void printhelp(Status src)
        {
            switch (src)
            {
                case Status.Menu:
                    foreach (string T in menu_available_actions) { Console.WriteLine(T); }
                    Console.WriteLine("\n");
                    break;
                case Status.Bataille:
                    foreach (string T in Bataille_available_actions) { Console.WriteLine(T); }
                    Console.WriteLine("\n");
                    break;
                case Status.BlackJack:
                    foreach (string T in BJ_available_actions) { Console.WriteLine(T); }
                    Console.WriteLine("\n");
                    break;
            }
        }

        void Print_turn_result(Status status, ProtocolCl scmd)
        {
            if (status == Status.Bataille)
            {
                switch (scmd.Command)
                {
                    case Cmd.Win:
                        Console.WriteLine("You won this turn. Your card was" + scmd.CardSend[0].ToString() + "Your oponent's card was" + scmd.CardSend[1].ToString());
                        break;
                    case Cmd.Lose:
                        Console.WriteLine("You lost this turn. Your card was" + scmd.CardSend[0].ToString() + "Your oponent's card was" + scmd.CardSend[1].ToString());
                        break;
                    case Cmd.Draw:
                        Console.WriteLine("This turn was a draw.");
                        break;
                }
            }
            else
            {

            }
        }

        private void Bataille()
        {
            try
            {
                int handnbr = 26;
                int pile = 0;

                ProtocolCl srv_cmd;
                NetworkComms.SendObject("MyPacket", IP, Port, new ProtocolCl(Cmd.Ready));
                Console.WriteLine("Waiting for ready players to launch the game !");
                srv_cmd = TCPconn.SendReceiveObject<ProtocolCl>("RequestCustomObject", "CustomObjectReply", 30000);
                if (srv_cmd.Command != Cmd.Ready)
                {
                    Console.WriteLine("Something wrong hapened, your game got destroyed.");
                    return;
                }
                Console.WriteLine("Write 'help' to get available commands");
                while (handnbr > 0 && handnbr < 52)
                {
                    string line = Console.ReadLine().ToLower();
                    switch (line)
                    {
                        case "help":
                            printhelp(Status.Bataille);
                            break;
                        case "" +
                        "hand":
                            NetworkComms.SendObject("MyPacket", IP, Port, new ProtocolCl(Cmd.Hand));
                            srv_cmd = TCPconn.SendReceiveObject<ProtocolCl>("RequestCustomObject", "CustomObjectReply", 30000);
                            break;
                        case "rdy":
                            NetworkComms.SendObject("MyPacket", IP, Port, new ProtocolCl(Cmd.Turn));
                            handnbr--;
                            pile += 2;
                            Print_turn_result(Status.Bataille, srv_cmd = TCPconn.SendReceiveObject<ProtocolCl>("RequestCustomObject", "CustomObjectReply", 30000));
                            if (srv_cmd.Command == Cmd.Win)
                            {
                                handnbr += pile;
                                pile = 0;
                            }
                            Console.WriteLine("Your have now " + handnbr + "cards in your hand !");
                            break;
                    }
                }
                if (handnbr == 0)
                    Console.WriteLine("You lost this game, try again !");
                else
                    Console.WriteLine("You won, congratz !");

            }
            catch (ExpectedReturnTimeoutException)
            {
                Console.WriteLine("Your game is not ready, it got destroyed...");
                return;
            }
        }

        private void BlackJack()
        {
            Console.WriteLine("BlackJack");
            Console.WriteLine("\n");
        }

        public void start()
        {
            TCPconn = TCPConnection.GetConnection(new ConnectionInfo(IP, Port));
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(dataSerializer, dataProcessors, dataProcessorOptions);
            TCPconn.AppendShutdownHandler(disconnect);

            Console.WriteLine("Write 'quit' to quit | Write 'help' to get available commands");
            while (true)
            {
                string line = Console.ReadLine().ToLower();
                if (line == "quit")
                    break;
                switch (line)
                {
                    case "help":
                        printhelp(Status.Menu);
                        break;
                    case "bj":
                        BlackJack();
                        break;
                    case "bt":
                        Bataille();
                        break;
                }
            }
        }

        private void disconnect(Connection conn)
        {
            Console.WriteLine("You got disconnected from the server");
            Environment.Exit(42);
        }
    }
}