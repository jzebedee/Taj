using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using MiscUtil.IO;

namespace Taj
{
    public static class Extensions
    {
        public static string ReadCString(this EndianBinaryReader reader)
        {
            var builder = new StringBuilder();

            char cur;
            while ((cur = (char)reader.ReadByte()) != '\0') builder.Append(cur);

            return builder.ToString();
        }

        public static T ReadStruct<T>(this EndianBinaryReader reader, int? Size = null) where T : struct
        {
            int structSize = Size ?? Marshal.SizeOf(typeof(T));
            byte[] readBytes = reader.ReadBytes(structSize);
            if (readBytes.Length != structSize)
                throw new ArgumentException("Size of bytes read did not match struct size");

            var pin_bytes = GCHandle.Alloc(readBytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(pin_bytes.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                pin_bytes.Free();
            }
        }
    }
}
