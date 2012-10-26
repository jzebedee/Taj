﻿using System.Text;
using Taj.Messages.Flags;

namespace Taj.Messages
{
    public class MH_Logon : MessageHeader, IOutgoingMessage
    {
        private const uint
            guest_puidCtr = 0xf5dc385e,
            guest_puidCRC = 0xc144c580;

        private readonly AuxRegistrationRec rec;

        public MH_Logon(PalaceConnection con, string name, short desiredRoom = 0, uint puidCtr = guest_puidCtr,
                        uint puidCRC = guest_puidCRC) : base(con)
        {
            rec = new AuxRegistrationRec
                      {
                          crc = 0x5905f923, //cribbed guest from OP
                          counter = 0xcf07309c, //cribbed guest from OP
                          userName = name.ToPString(31),
                          wizPassword = string.Empty.ToPString(31),
                          auxFlags = AuxFlags.Authenticate | AuxFlags.Win32,
                          puidCtr = puidCtr, //cribbed guest from OP
                          puidCRC = puidCRC, //cribbed guest from OP

                          demoElapsed = 0, //garbage
                          totalElapsed = 0, //garbage
                          demoLimit = 0, //garbage

                          desiredRoom = desiredRoom,
                          reserved = Encoding.GetEncoding("iso-8859-1").GetBytes("OPNPAL"),
                          ulRequestedProtocolVersion = 0,
                          ulUploadCaps = ulUploadCapsFlags.ASSETS_PALACE,
                          ulDownloadCaps = ulDownloadCapsFlags.ASSETS_PALACE | ulDownloadCapsFlags.FILES_PALACE | ulDownloadCapsFlags.FILES_HTTPSrvr,
                          ul2DEngineCaps = ul2DEngineCapsFlags.NONE,
                          ul2DGraphicsCaps = ul2DGraphicsCapsFlags.NONE,
                          ul3DEngineCaps = ul3DEngineCapsFlags.NONE,
                      };
        }

        public MH_Logon(PalaceConnection con) : base(con)
        {
            rec = Reader.ReadStruct<AuxRegistrationRec>();
        }

        #region IOutgoingMessage Members

        public void Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.LOGON,
                                       length = 128,
                                       refNum = 0, //intentional
                                   });
            Writer.WriteStruct(rec);

            Writer.Flush();
        }

        #endregion
    }
}