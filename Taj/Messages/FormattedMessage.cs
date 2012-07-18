using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages
{
    public class FormattedMessage
    {
        byte[] fmBuffer;
        protected unsafe void Set(ClientMsg msg, byte* content)
        {
            fmBuffer = new byte[(sizeof(ClientMsg) + msg.length)];
            fixed (byte* pBuf = fmBuffer)
            {
                *((ClientMsg*)pBuf) = msg;

                byte* pB = pBuf + sizeof(ClientMsg);
                for (int i = 0; i < msg.length; i++)
                    *pB++ = *content++;
            }
        }

        public byte[] GetBytes()
        {
            return fmBuffer;
        }
    }
}