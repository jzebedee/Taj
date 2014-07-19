using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Taj
{
    public class Clever
    {
        #region Constants
        private const string URL = "http://cleverbot.com/webservicemin";
        #endregion

        #region Fields
        Dictionary<string, string> postParameters = new Dictionary<string, string>();
        #endregion

        #region Construction
        public Clever()
        {
            postParameters.Add("start", "y");
            postParameters.Add("icognoid", "wsf");
            postParameters.Add("fno", "0");
            postParameters.Add("sub", "Say");
            postParameters.Add("islearning", "1");
            postParameters.Add("cleanslate", "false");
        } 
        #endregion

        #region Think
        public string Think(string input)
        {
            postParameters["stimulus"] = input;

            string formData = HttpParams(postParameters);
            string formDataSub = formData.Substring(9, 20);
            string formDataDigest = MD5(formDataSub);

            postParameters["icognocheck"] = formDataDigest;
            string strResponse = HttpPost(postParameters);
            string[] arrResponse = strResponse.Split('\r');

            postParameters["sessionid"] = StringAtIndex(arrResponse, 1);
            postParameters["logurl"] = StringAtIndex(arrResponse, 2);

            postParameters["vText8"] = StringAtIndex(arrResponse, 3);
            postParameters["vText7"] = StringAtIndex(arrResponse, 4);
            postParameters["vText6"] = StringAtIndex(arrResponse, 5);
            postParameters["vText5"] = StringAtIndex(arrResponse, 6);
            postParameters["vText4"] = StringAtIndex(arrResponse, 7);
            postParameters["vText3"] = StringAtIndex(arrResponse, 8);
            postParameters["vText2"] = StringAtIndex(arrResponse, 9);
            postParameters["prevref"] = StringAtIndex(arrResponse, 10);

            postParameters["emotionalhistory"] = StringAtIndex(arrResponse, 12);
            postParameters["ttsLocMP3"] = StringAtIndex(arrResponse, 13);
            postParameters["ttsLocTXT"] = StringAtIndex(arrResponse, 14);
            postParameters["ttsLocTXT3"] = StringAtIndex(arrResponse, 15);
            postParameters["ttsText"] = StringAtIndex(arrResponse, 16);
            postParameters["lineRef"] = StringAtIndex(arrResponse, 17);
            postParameters["lineURL"] = StringAtIndex(arrResponse, 18);
            postParameters["linePOST"] = StringAtIndex(arrResponse, 19);
            postParameters["lineChoices"] = StringAtIndex(arrResponse, 20);
            postParameters["lineChoicesAbbrev"] = StringAtIndex(arrResponse, 21);
            postParameters["typingData"] = StringAtIndex(arrResponse, 22);
            postParameters["divert"] = StringAtIndex(arrResponse, 23);

            return StringAtIndex(arrResponse, 16);
        }
        #endregion

        #region MD5
        private static string MD5(string input)
        {
            MD5CryptoServiceProvider cr = new MD5CryptoServiceProvider();
            byte[] source = Encoding.UTF8.GetBytes(input);
            byte[] hash = cr.ComputeHash(source);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        } 
        #endregion

        #region HttpPost
        private static string HttpParams(IDictionary<string, string> p)
        {
            string urlData = string.Empty;
            List<string> urlDataList = new List<string>();
            foreach (string pkey in p.Keys)
            {
                string pval = p[pkey];
                urlDataList.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(pkey), HttpUtility.UrlEncode(pval)));
            }
            urlData = string.Join("&", urlDataList.ToArray());
            return urlData;
        }

        private static string HttpPost(IDictionary<string, string> p)
        {
            string result = string.Empty;

            string urlData = HttpParams(p);
            byte[] urlDataPost = Encoding.ASCII.GetBytes(urlData);

            WebRequest request = WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = urlDataPost.Length;

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(urlDataPost, 0, urlDataPost.Length);
            reqStream.Close();

            WebResponse response = request.GetResponse();
            using (StreamReader streamRd = new StreamReader(response.GetResponseStream()))
            {
                result = streamRd.ReadToEnd().Trim();
            }

            return result;
        } 
        #endregion

        #region StringAtIndex
        private static string StringAtIndex(string[] arr, int index)
        {
            if (index >= arr.Length)
                return string.Empty;
            return arr[index];
        }
        #endregion
    }
}
