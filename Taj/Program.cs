using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Taj
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var identity = new PalaceUser {Name = "Superduper"};

            //var pal = new Palace(new Uri("tcp://chat.epalaces.com:9998"));
            var pal = new PalaceConnection(new Uri("tcp://oceansapart.epalaces.com:9998"), identity);

#if TRACE
            File.Delete("trace.log");
            var file_log_listener = new TextWriterTraceListener("trace.log");
            var console_listener = new ConsoleTraceListener();
            Trace.Listeners.Add(file_log_listener);
            Trace.Listeners.Add(console_listener);
            Trace.AutoFlush = true;
#endif

            while (true)
                Thread.Sleep(1000);
            //while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }
    }
}