using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj
{
    class Program
    {
        static void Main(string[] args)
        {
            var pal = new Palace(new Uri("tcp://chat.epalaces.com:9998"));

            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }
    }
}
