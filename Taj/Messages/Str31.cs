using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Str31
    {
        public byte length;
        public fixed sbyte msg[31];

        public string Content
        {
            get
            {
                fixed (sbyte* pMsg = msg)
                    return new string(pMsg, 0, length);
            }
            set
            {
                if (value.Length > 31)
                    throw new ArgumentOutOfRangeException();

                length = Convert.ToByte(value.Length);

                fixed (sbyte* pMsg = msg)
                {
                    sbyte* rpMsg = pMsg;
                    foreach (var b in Encoding.ASCII.GetBytes(value))
                        *rpMsg++ = (sbyte)b;
                }
            }
        }
    }
}
