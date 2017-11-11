using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using NetworkCommsDotNet;

namespace cardGame_Server
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
            Server srv = new Server();
            srv.Start();
        }

        static bool ConsoleEventCallback(int evt)
        {
            NetworkComms.Shutdown();
            return false;
        }
    }
}
