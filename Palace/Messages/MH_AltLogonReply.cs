using System.Text;
using Palace;
using Palace.Messages.Flags;
using Palace.Messages.Structures;
using System;

namespace Palace.Messages
{
    public class MH_AltLogonReply : MessageWriter
    {
        public MH_AltLogonReply(ClientMessage header, byte[] backing)
            : base(header, backing) { }

        #region IOutgoingMessage Members

        public byte[] Write(int userID, AuxRegistrationRec Record)
        {
            Writer.WriteStruct(new ClientMessage
                                {
                                    eventType = MessageTypes.ALTLOGONREPLY,
                                    length = AuxRegistrationRec.Size,
                                    refNum = userID,
                                });
            Writer.WriteStruct(Record);
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}