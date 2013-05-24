using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Cotserve
{
    class Program
    {
        static void Main(string[] args)
        {
            var listen = new TcpListener(IPAddress.Loopback, 9991);
            try
            {
                while (true)
                {
                    listen.Start(1);

                    Console.WriteLine("Listening started.");
                    using (var client = listen.AcceptTcpClient())
                    {
                        while (client.Connected)
                        {
                            Console.WriteLine("Client connected.");
                            using (var stream = client.GetStream())
                            {
                                do
                                {
                                    var rbte = stream.ReadByte();
                                    Console.WriteLine(rbte);
                                } while (true);
                            }
                        }
                    }
                    Console.WriteLine("NOTCHA");
                }
            }
            finally
            {
                listen.Stop();
            }

            Console.ReadKey();
        }
    }
}
