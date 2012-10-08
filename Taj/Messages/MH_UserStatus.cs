using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Taj.Messages
{
    public class MH_UserStatus : MessageHeader
    {
        public PalaceUser Target { get; private set; }

        public MH_UserStatus(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            //var tbytes = Reader.ReadBytes(tlen);
            //using (var writer = new BinaryWriter(File.OpenWrite("userstatus.dump")))
            //    writer.Write(tbytes);
            //
            //throw new Exception();

            Target = Palace.GetUserByID(cmsg.refNum, true);
            Target.Flags = (UserFlags)Reader.ReadInt16();

            var unk1 = Reader.ReadInt16();
            var unk2_mnem = Encoding.ASCII.GetString(Reader.ReadBytes(4)); //dneS
            var unk3 = Reader.ReadInt32();
            var unk4 = Reader.ReadInt32(); //userid
            var unk5 = Reader.ReadInt32();

            var unk6a = Reader.ReadInt32();
            var unk6b = Reader.ReadInt32();

            //var unk6c = Reader.ReadBytes(6);
            //var unk6d = Reader.ReadBytes(6);

            var unk7 = Reader.ReadInt32();
            var unk8 = Reader.ReadInt32();
            var unk9 = Reader.ReadInt32();
            var unk10 = Reader.ReadInt32();

            //var unk10 = Reader.ReadInt32();
            //var unk11 = Reader.ReadInt16();// 32();
        }
    }
}