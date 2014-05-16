using System.Diagnostics;
using Palace.Messages.Structures;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Palace.Messages
{
    /// <summary>
    /// This message is used to change a user’s props. A client sends a MSG_USERPROP
    /// message to the server requesting the change. If the operation is successful, the
    /// server sends a matching MSG_USERPROP message to the other clients in the room,
    /// informing them of the event. In both directions the message format is the same.
    /// </summary>
    public class MH_UserProp : MessageReader
    {
        public MH_UserProp(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            UserID = cmsg.refNum; //The refnum field contains the UserID of the user whose props are changing

            var numProps = Reader.ReadInt32();
            Debug.WriteLine("{0} changed props (new count: {1})", UserID, numProps);

            PropIDs = new List<int>();

            var pSpecList = new List<AssetSpec>(9);
            while (numProps-- > 0)
            {
                var spec = Reader.ReadStruct<AssetSpec>();
                pSpecList.Add(spec);

                Debug.WriteLine("{0}th Prop spec: ID {1} CRC {2}", numProps, spec.id, spec.crc);

                Debug.WriteLine("Issuing prop request to palace server");
                var MHx = new MH_AssetQuery();
                MHx.Write(UserID, spec.id, spec.crc);

                //GetWebPropAsync(spec.id);
                PropIDs.Add(spec.id);
            }

            //user.PropRecords = pSpecList;
        }

        public int UserID { get; private set; }
        public List<int> PropIDs { get; private set; }

        //private async Task<PalaceProp> DownloadAsync(int id)
        //{
        //    try
        //    {
        //        var WCPropFetch = new WebClient();
        //        var propuri = Palace.HTTPServer + @"/webservice/storage/" + id;

        //        var propDLTask = WCPropFetch.DownloadDataTaskAsync(propuri);

        //        return new PalaceProp(await propDLTask, id, skip: true);
        //    }
        //    catch (WebException)
        //    {
        //        return null;
        //    }
        //}

        //private async void GetWebPropAsync(int id)
        //{
        //    var httpsrv = Palace.HTTPServer;
        //    if (!string.IsNullOrWhiteSpace(httpsrv))
        //    {
        //        Debug.WriteLine("Issuing prop request to HTTP server " + httpsrv);

        //        var prop = await DownloadAsync(id);
        //        if (prop != null)
        //        {
        //            Debug.WriteLine("Received prop response from HTTP request");
        //            prop.Save(Assets);
        //        }
        //        else
        //            Debug.WriteLine("Null prop response for HTTP request");
        //    }
        //}
    }
}