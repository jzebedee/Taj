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

            byte cur;
            while ((cur = reader.ReadByte()) != '\0') builder.Append((char)cur);

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

        public static void WriteStruct<T>(this EndianBinaryWriter writer, T obj) where T : struct
        {
            var size = Marshal.SizeOf(obj);
            byte[] buf = new byte[size];

            IntPtr ptrAlloc = IntPtr.Zero;

            try
            {
                ptrAlloc = Marshal.AllocHGlobal(size);

                Marshal.StructureToPtr(obj, ptrAlloc, true);
                Marshal.Copy(ptrAlloc, buf, 0, size);
            }
            finally
            {
                if (ptrAlloc != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptrAlloc);
            }

            writer.Write(buf);
        }

        public static byte[] ToStr31(this string str)
        {
            if (str.Length > 31)
                str.Substring(0, 31);

            byte[] ret = new byte[32];
            ret[0] = Convert.ToByte(str.Length);

            Array.Copy(Encoding.GetEncoding("Windows-1252").GetBytes(str), 0, ret, 1, str.Length);
            return ret;
        }
    }
}
