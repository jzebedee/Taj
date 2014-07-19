using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MiscUtil.IO;

namespace Palace
{
    public static class Extensions
    {
        public static byte[] ChainAppendBuffer(params byte[][] buffers)
        {
            var allBytes = new byte[buffers.Sum(buf => buf.Length)];

            int i = 0;
            foreach (var buf in buffers)
            {
                Buffer.BlockCopy(buf, 0, allBytes, i, buf.Length);
                i += buf.Length;
            }

            return allBytes;
        }

        public static string ReadCString(this EndianBinaryReader reader)
        {
            var builder = new StringBuilder();

            byte cur;
            while ((cur = reader.ReadByte()) != '\0') builder.Append((char)cur);

            return builder.ToString();
        }

        public static string MarshalCString(this byte[] buf, int offset = 0)
        {
            buf = TrimBuffer(buf, offset);

            var builder = new StringBuilder();

            byte cur;
            for (int i = 0; ((cur = buf[i]) != '\0'); i++)
                builder.Append((char)cur);

            return builder.ToString();
        }

        public static string ReadPString(this EndianBinaryReader reader)
        {
            var builder = new StringBuilder();

            byte length = reader.ReadByte();
            while (length-- > 0) builder.Append((char)reader.ReadByte());

            return builder.ToString();
        }

        public static string MarshalPString(this byte[] buf, int offset = 0)
        {
            byte length = buf[offset];

            buf = TrimBuffer(buf, offset + 1, length);

            var builder = new StringBuilder();

            for (byte i = length; i > 0; ) builder.Append((char)buf[length - i--]);

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

        public static T MarshalStruct<T>(this byte[] buf, int offset = 0, int length = -1) where T : struct
        {
            buf = TrimBuffer(buf, offset, length);

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

        public static byte[] TrimBuffer(this byte[] buf, int offset, int length = -1)
        {
            if (offset == 0 && length < 0)
                return buf;

            var newLength = (length < 0 ? buf.Length - offset : length);
            var newBuf = new byte[newLength];
            Array.Copy(buf, offset, newBuf, 0, newLength);

            return newBuf;
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

            Array.Copy(Encoding.UTF8.GetBytes(str), 0, ret, 1, str.Length);
            return ret;
        }

        public static string ToArrayString(this byte[] buffer)
        {
            return "[" + buffer.Aggregate("", (str, b) => str + b.ToString("X") + ",").TrimEnd(',') + "]";
        }
    }
}