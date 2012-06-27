using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net.Sockets;

namespace Taj
{
    public class Palace
    {
        public Palace(Uri target)
        {
            using (var socket = new TcpClient(target.Host, target.Port))
            {
                using (var netstream = socket.GetStream())
                {
                    byte[] buf;

                    do
                    {
                        while (netstream.DataAvailable)
                        {
                            buf = new byte[1024];
                            netstream.Read(buf, 0, 1024);

                            Console.WriteLine(Encoding.ASCII.GetString(buf));
                        }
                        
                        System.Threading.Thread.Sleep(100);
                    } while (socket.Connected);
                }
            }
        }
    }
}
