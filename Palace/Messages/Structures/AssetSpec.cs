﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AssetSpec
    {
        public Int32 id; //supposed to be int32
        public Int32 crc;
    }
}
