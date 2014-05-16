using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Palace.Messages.Flags;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_UserStatus : MessageReader
    {
        public MH_UserStatus(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            UserID = cmsg.refNum;
            UserFlags = (UserFlags)Reader.ReadInt16();

            /*
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
            */

            Debug.WriteLine("Target: {0}", UserID);
            Debug.WriteLine("Flags: {0}", UserFlags);
        }

        public int UserID { get; private set; }
        public UserFlags UserFlags { get; private set; }
    }
}