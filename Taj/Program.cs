using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if TRACE
using System.Diagnostics;
#endif

namespace Taj
{
    class Program
    {
        static void Main(string[] args)
        {
            //var pal = new Palace(new Uri("tcp://chat.epalaces.com:9998"));
            var pal = new Palace(new Uri("tcp://oceansapart.epalaces.com:9998"));

#if TRACE
            var file_log_listener = new TextWriterTraceListener("trace.log");
            var console_listener = new ConsoleTraceListener();
            Trace.Listeners.Add(file_log_listener);
            Trace.Listeners.Add(console_listener);
#endif

            while (true)
                System.Threading.Thread.Sleep(1000);
            //while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }
    }
}
