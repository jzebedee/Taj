using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Palace;

namespace Cotserve
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serv = new Server())
            {
                Console.WriteLine("Starting server...");
                serv.Start();
                Console.WriteLine("Listening. Press any key to stop.");
                Console.ReadKey();
                Console.WriteLine("Stopping server...");
                serv.Stop();
                Console.WriteLine("Stopped.");
            }

            Console.WriteLine("Fin. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
