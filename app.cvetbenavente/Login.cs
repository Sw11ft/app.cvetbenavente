using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace app.cvetbenavente
{
    public static class Login
    {
        public enum LoginStatus
        {
            Success,
            Fail,
            NoConnection
        }

        public static string GetAccessKey(string username, string password)
        {
            string strUri = "https://api.cvb.pedro-gaspar.com/api/token";
            Uri Uri = new Uri(strUri);
            string data = "username=" + username + "&password=" + password;

            string result;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    result = wc.UploadString(Uri, data);
                }
                catch (Exception e) //Ex: Bad Request
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            if ((result.StartsWith("[") && result.EndsWith("]")) ||
                (result.StartsWith("{") && result.EndsWith("}")))
            {
                try
                {
                    var jsonResult = JToken.Parse(result);
                    var accessToken = jsonResult["access_token"].ToString();

                    return string.IsNullOrWhiteSpace(accessToken) ? null : accessToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            return null;
        }

        public static string GetEvents(string accessKey, DateTime from, DateTime to)
        {
            string strUri = string.Format("https://api.cvb.pedro-gaspar.com/api/eventos?from={0}&to={1}", 
                                           Uri.EscapeDataString(from.ToString("MM/dd/yyyy")), 
                                           Uri.EscapeDataString(to.ToString("MM/dd/yyyy")));

            string result;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessKey;

                try
                {
                    result = wc.DownloadString(strUri);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    return null;
                }
            }
        }
    }
}