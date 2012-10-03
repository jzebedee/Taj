using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj
{
    public class PalaceEncryption
    {
        static Lazy<short[]> seeded = new Lazy<short[]>(() => {
            int seed = 0xa2c2a;
            var lut = new short[512];

            for (int i = 0; i < 512; i++)
            {
                int a = 16807 * (seed % 0x1f31d) - 2836 * (seed / 0x1f31d);
                lut[i] = (short)((seed = a > 0 ? a : a + 0x7fffffff) / 2147483647d * 256d);
            }

            return lut;
        });

        public byte[] Encrypt(string message, bool utf8Output = false, int byteLimit = 254)
        {
            byte[] ba = (utf8Output ? Encoding.GetEncoding("UTF-8") : Encoding.GetEncoding("Windows-1252")).GetBytes(message);
            if (ba.Length > byteLimit)
                Array.Resize(ref ba, byteLimit);

            var bs = new byte[ba.Length];
            byte lastChar = 0;

            int rc = 0;
            for (int i = ba.Length - 1; i >= 0; i--)
            {
                bs[i] = (byte)(ba[i] ^ seeded.Value[rc++] ^ lastChar);
                lastChar = (byte)(bs[i] ^ seeded.Value[rc++]);
            }

            return bs;
        }

        public string Decrypt(byte[] bytesIn, bool utf8Input = false)
        {
            var bs = new byte[bytesIn.Length];
            byte lastChar = 0;

            int rc = 0;
            for (int i = bs.Length - 1; i >= 0; i--)
            {
                bs[i] = (byte)(bytesIn[i] ^ seeded.Value[rc++] ^ lastChar);
                lastChar = (byte)(bytesIn[i] ^ seeded.Value[rc++]);
            }

            return (utf8Input ? Encoding.GetEncoding("UTF-8") : Encoding.GetEncoding("Windows-1252")).GetString(bs);
        }
    }
}