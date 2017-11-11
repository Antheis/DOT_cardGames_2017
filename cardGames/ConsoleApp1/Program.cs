using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace cardGame_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server srv = new Server();
            srv.Start();
        }
    }
}
