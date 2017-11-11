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
        }
    }
}
