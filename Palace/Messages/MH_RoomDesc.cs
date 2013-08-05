using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MiscUtil.IO;
using Palace.Messages.Flags;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_RoomDesc : MessageHeader
    {
        public MH_RoomDesc(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var roomrec = Reader.ReadStruct<RoomRec>();
            var room = Palace.GetRoomByID(roomrec.roomID);

            room.Flags = roomrec.roomFlags;
            room.FacesID = roomrec.facesID;

            var varbuf = Reader.ReadBytes(roomrec.lenVars); //varBuf is an array of lenVars bytes of variable-length data associated with the RoomRec, as described above

            room.Name = varbuf.MarshalPString(roomrec.roomNameOfst); //roomNameOfst is the index into varBuf of a PString that is the name of the room.

            var pictName = varbuf.MarshalPString(roomrec.pictNameOfst); //pictNameOfst is the index into varBuf of a PString that is the filename of a picture to use as the room background.

            var artistName = varbuf.MarshalPString(roomrec.artistNameOfst); //artistNameOfst is the index into varBuf of a PString that is the name of the artist who created the room.

            var password = varbuf.MarshalPString(roomrec.passwordOfst); //passwordOfst is the index into varBuf of a PString that is the password for the room -- member-created rooms may have passwords for controlling entry to the room.

            var hotspots = new HotspotRec[roomrec.nbrHotspots]; //nbrHotspots is the number of hotspots in the room.
            for (int i = 0; i < roomrec.nbrHotspots; i++)
                hotspots[i] = varbuf.MarshalStruct<HotspotRec>(roomrec.hotspotOfst + (HotspotRec.Size * i), HotspotRec.Size); //hotspotOfst is the index into varBuf of the beginning of an array of nbrHotspots Hotspot structs (described below) that describe the hotspots in the room. Note that this array must be aligned on a 4-byte boundary.

            var pictures = new PictureRec[roomrec.nbrPictures]; //nbrPictures is the number of pictures in the room
            for (int i = 0; i < roomrec.nbrPictures; i++)
                pictures[i] = varbuf.MarshalStruct<PictureRec>(roomrec.pictureOfst + (PictureRec.Size * i), PictureRec.Size); //pictureOfst is the index into varBuf of the beginning of an array of nbrPictures PictureRec structs (described below) that describe the pictures in the room. Note that this array must be aligned on a 4-byte boundary.

            var drawRecords = new Tuple<DrawRec, byte[]>[roomrec.nbrDrawCmds]; //nbrDrawCmds is the number of draw commands in the room’s display list.
            int drawStructOffset = roomrec.firstDrawCmd; //firstDrawCmd is the index into varBuf of the first of a packed sequence of nbrDrawCmds draw commands (DrawRecord structs and attached data, see §3.9. MSG_DRAW). Note that these records must be aligned on 4-byte boundaries.
            for (int i = 0; i < roomrec.nbrDrawCmds; i++)
            {
                var curDrawRec = varbuf.MarshalStruct<DrawRec>(drawStructOffset, DrawRec.Size);
                var curCmdData = varbuf.TrimBuffer(curDrawRec.dataOfst, curDrawRec.cmdLength);
                drawRecords[i] = Tuple.Create(curDrawRec, curCmdData);
                drawStructOffset = curDrawRec.link.nextOfst;
            }

            var userCount = roomrec.nbrPeople; //nbrPeople is the number of users currently in the room.

            var props = new LPropRec[roomrec.nbrLProps]; //nbrLProps is the number of props in the room.
            int propStructOffset = roomrec.firstLProp; //firstLProp is the index into varBuf of the beginning of an array of nbrLProps LPropRec structs (described below) that describe the props in the room. Note that this array must be aligned on a 4-byte boundary.
            for (int i = 0; i < roomrec.nbrLProps; i++)
            {
                var curPropRec = varbuf.MarshalStruct<LPropRec>(propStructOffset, LPropRec.Size);
                props[i] = curPropRec;
                propStructOffset = curPropRec.link.nextOfst;

                var MHx = new MH_AssetQuery(con, props[i].propSpec.id, props[i].propSpec.crc);
                MHx.Write();
            }

            var finalread = Reader.ReadInt32();

            Palace.CurrentRoom = room;
        }
    }
}