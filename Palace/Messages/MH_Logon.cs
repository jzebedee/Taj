//#define OPENPALACE
using System.Text;
using Palace;
using Palace.Messages.Flags;
using Palace.Messages.Structures;
using Regcode = Palace.PalaceRegistration.RegistrationCode;

namespace Palace.Messages
{
    public class MH_Logon : MessageHeader, IOutgoingMessage
    {
#if OPENPALACE
        private static readonly Regcode
            OPENPALACE_GUEST = new Regcode(0x5905f923, 0xcf07309c),
            OPENPALACE_GUEST_PUID = new Regcode(0x5905f923, 0xcf07309c);
#endif

        public AuxRegistrationRec Record { get; private set; }

        public MH_Logon(IPalaceConnection con, string name, short desiredRoom = 0)
            : base(con)
        {
            var reg = PalaceRegistration.Generate();
            var reg2 = PalaceRegistration.Generate(); //TODO: make perm

            Record = new AuxRegistrationRec
            {
#if OPENPALACE
                counter = OPENPALACE_GUEST.Counter,
                crc = OPENPALACE_GUEST.CRC,
#else
                counter = reg.Counter, //cribbed guest from OP
                crc = reg.CRC, //cribbed guest from OP
#endif
                userName = name.ToPString(31),
                wizPassword = string.Empty.ToPString(31),
                auxFlags = AuxFlags.Authenticate | AuxFlags.Win32,
#if OPENPALACE
                puidCtr = OPENPALACE_GUEST.Counter, //cribbed guest from OP
                puidCRC = OPENPALACE_GUEST.CRC, //cribbed guest from OP
#else
                puidCtr = reg2.Counter,
                puidCRC = reg2.CRC,
#endif

                demoElapsed = 0, //garbage
                totalElapsed = 0, //garbage
                demoLimit = 0, //garbage

                desiredRoom = desiredRoom,
#if OPENPALACE
                reserved = Encoding.GetEncoding("iso-8859-1").GetBytes("OPNPAL"),
#endif
                reserved = Encoding.GetEncoding("iso-8859-1").GetBytes("PC4125"),
                ulRequestedProtocolVersion = 0,
                ulUploadCaps = ulUploadCapsFlags.ASSETS_PALACE,
                ulDownloadCaps = ulDownloadCapsFlags.ASSETS_PALACE | ulDownloadCapsFlags.FILES_PALACE | ulDownloadCapsFlags.FILES_HTTPSrvr,
                ul2DEngineCaps = ul2DEngineCapsFlags.PALACE, //PALACE
                ul2DGraphicsCaps = ul2DGraphicsCapsFlags.GIF87, //GIF87
                ul3DEngineCaps = ul3DEngineCapsFlags.NONE,
            };
        }

        public MH_Logon(IPalaceConnection con)
            : base(con)
        {
            Record = Reader.ReadStruct<AuxRegistrationRec>();
        }

        #region IOutgoingMessage Members

        public virtual void Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.LOGON,
                                       length = AuxRegistrationRec.Size,
                                       refNum = 0, //intentional
                                   });
            Writer.WriteStruct(Record);

            Writer.Flush();
        }

        #endregion
    }
}