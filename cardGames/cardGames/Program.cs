using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cardGames
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the server IP and port in the format IP:Port");
            string src = Console.ReadLine();

            string  IP = src.Split(':').First();
            int     Port = int.Parse(src.Split(':').Last());

            int loopCounter = 1;
            while (true)
            {

                //Send the message in a single line
                NetworkComms.SendObject("Message", IP, Port, messageToSend);

                //Check if user wants to go around the loop
                Console.WriteLine("\nPress q to quit or any other key to send another message.");
                if (Console.ReadKey(true).Key == ConsoleKey.Q) break;
                else loopCounter++;
            }

            //We have used comms so we make sure to call shutdown
            NetworkComms.Shutdown();
        }
    }
}
