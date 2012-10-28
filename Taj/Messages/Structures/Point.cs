using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        Int16 y; //v
        Int16 x; //h
    }
}
