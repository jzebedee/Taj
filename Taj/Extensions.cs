﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

        public static string ReadPString(this EndianBinaryReader reader)
        {
            var builder = new StringBuilder();

            byte length = reader.ReadByte();
            while (length-- > 0) builder.Append((char)reader.ReadByte());

            return builder.ToString();
        }

        public static T ReadStruct<T>(this EndianBinaryReader reader, int? Size = null) where T : struct
        {
            int structSize = Size ?? Marshal.SizeOf(typeof(T));
            byte[] readBytes = reader.ReadBytes(structSize);
            if (readBytes.Length != structSize)
                throw new ArgumentException("Size of bytes read did not match struct size");

            return readBytes.MarshalStruct<T>();
        }

        public static async Task<T> ReadStructAsync<T>(this EndianBinaryReader reader, int? Size = null) where T : struct
        {
            int structSize = Size ?? Marshal.SizeOf(typeof(T));
            byte[] readBytes = await reader.ReadBytesAsync(structSize);
            if (readBytes.Length != structSize)
                throw new ArgumentException("Size of bytes read did not match struct size");

            return readBytes.MarshalStruct<T>();
        }

        public static T MarshalStruct<T>(this byte[] buf) where T : struct
        {
            GCHandle pin_bytes = GCHandle.Alloc(buf, GCHandleType.Pinned);
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
            int size = Marshal.SizeOf(obj);
            var buf = new byte[size];

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

        public static byte[] ToPString(this string str, int length)
        {
            if (str.Length > length)
                str = str.Substring(0, length);

            var ret = new byte[length + 1];
            ret[0] = Convert.ToByte(str.Length);

            Array.Copy(Encoding.GetEncoding("Windows-1252").GetBytes(str), 0, ret, 1, str.Length);
            return ret;
        }
    }
}