using Palace.Messages.Structures;
using System;
using System.Diagnostics;
namespace Palace.Messages
{
    public class MH_XWhisper : MessageWriter
    {
        public MH_XWhisper(PalaceUser target, string msg)
        {
            Trace.Assert(msg.Length <= 255);

            Text = msg;
            TargetID = target.ID;
        }
        public MH_XWhisper(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            short len = Reader.ReadInt16();
            byte[] xmsg = Reader.ReadBytes(len - 3);
            Reader.ReadByte();
            Text = PalaceEncryption.Decrypt(xmsg);
            TargetID = cmsg.refNum;
        }

        public string Text { get; private set; }
        public int TargetID { get; private set; }

        #region IOutgoingMessage Members

        public byte[] Write(int myID)
        {
            byte[] xmsg = PalaceEncryption.Encrypt(Text);

            Writer.WriteStruct(new ClientMessage
                                {
                                    eventType = MessageTypes.XWHISPER,
                                    length = sizeof(int) + sizeof(short) + xmsg.Length + 1,
                                    refNum = myID,
                                }
                              );
            Writer.Write(TargetID);
            Writer.Write((short)(xmsg.Length + 3));
            Writer.Write(xmsg);
            Writer.Write((byte)0);
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}