using System.Text;
using Palace;
using Palace.Messages.Flags;
using Palace.Messages.Structures;
using System;

namespace Palace.Messages
{
    public class MH_AltLogonReply : MH_Logon
    {
        public PalaceUser User { get; set; }

        public MH_AltLogonReply(IPalaceConnection con)
            : base(con)
        {
        }

        #region IOutgoingMessage Members

        public override void Write()
        {
            if (User == null)
                throw new ArgumentNullException("User");

            Writer.WriteStruct(new ClientMessage
                                {
                                    eventType = MessageTypes.ALTLOGONREPLY,
                                    length = AuxRegistrationRec.Size,
                                    refNum = User.ID,
                                });
            Writer.WriteStruct(Record);

            Writer.Flush();
        }

        #endregion
    }
}