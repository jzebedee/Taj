﻿using Palace.Messages.Structures;
using System;
namespace Palace.Messages
{
    public class MH_XWhisper : MessageHeader, IOutgoingMessage
    {
        public readonly PalaceUser Target;

        public MH_XWhisper(IPalaceConnection con, PalaceUser target, string msg)
            : base(con)
        {
            //TODO: handle >255 messages
            if (msg.Length > 255)
                throw new ArgumentException();
                //msg = msg.Substring(0, 255);

            Text = msg;
            Target = target;
        }

        public MH_XWhisper(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            short len = Reader.ReadInt16();
            byte[] xmsg = Reader.ReadBytes(len - 3);
            Reader.ReadByte();
            Text = PalaceEncryption.Decrypt(xmsg);

            Target = Palace.GetUserByID(cmsg.refNum, true);
        }

        public string Text { get; private set; }

        #region IOutgoingMessage Members

        public void Write()
        {
            byte[] xmsg = PalaceEncryption.Encrypt(Text);

            Writer.WriteStruct(new ClientMessage
                                {
                                    eventType = MessageTypes.XWHISPER,
                                    length = sizeof(int) + sizeof(short) + xmsg.Length + 1,
                                    refNum = CurrentUser.ID,
                                }
                              );
            Writer.Write(Target.ID);
            Writer.Write((short)(xmsg.Length + 3));
            Writer.Write(xmsg);
            Writer.Write((byte)0);
            Writer.Flush();
        }

        #endregion
    }
}