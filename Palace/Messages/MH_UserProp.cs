﻿using System.Diagnostics;
using Palace.Messages.Structures;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Palace.Messages
{
    /// <summary>
    /// This message is used to change a user’s props. A client sends a MSG_USERPROP
    /// message to the server requesting the change. If the operation is successful, the
    /// server sends a matching MSG_USERPROP message to the other clients in the room,
    /// informing them of the event. In both directions the message format is the same.
    /// </summary>
    public class MH_UserProp : MessageHeader
    {
        public MH_UserProp(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var user = Palace.GetUserByID(cmsg.refNum); //The refnum field contains the UserID of the user whose props are changing

            var numProps = Reader.ReadInt32();
            Debug.WriteLine("{0} changed props (new count: {1})", user, numProps);

            while (numProps-- > 0)
            {
                var spec = Reader.ReadStruct<AssetSpec>();

                Debug.WriteLine("{0}th Prop spec: ID {1} CRC {2}", numProps, spec.id, spec.crc);

                Debug.WriteLine("Issuing prop request to palace server");
                var MHx = new MH_AssetQuery(con, spec.id, spec.crc);
                MHx.Write();

                var httpsrv = Palace.HTTPServer;
                if (!string.IsNullOrWhiteSpace(httpsrv))
                {
                    Debug.WriteLine("Issuing prop request to HTTP server " + httpsrv);

                    Download(spec.id);
                }
            }
        }

        private async Task<PalaceProp> Download(int id)
        {
            try
            {
                var WCPropFetch = new WebClient();
                var propuri = Palace.HTTPServer + @"/webservice/storage/" + id;

                var propDLTask = WCPropFetch.DownloadDataTaskAsync(propuri);

                return new PalaceProp(await propDLTask, AssetType.PROP, id, skip: true);
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}