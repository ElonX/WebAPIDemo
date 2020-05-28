using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace WebServerDemo.Controllers
{
    public class tokenController : ApiController
    {
        //当前示例中只设定一个合法用户U123456
        string userA_APPID = "U123456";
        public static string userA_access_token = string.Empty;
        public static long userA_expires_timestamp = 0;

        string HCAMKEY = "AABBCCDD";
        int expires_in = 3600;//有效期一小时

        public static long nowTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        private static bool isValidRequest(long timestamp)//请求有效期+5-5分钟
        {
            long nowts = nowTimeStamp();
            return (nowts + 300 >= timestamp) && (nowts - timestamp < 300);
        }

        private bool authenticate(string appid, string timestamp, string sign, out string error)
        {
            if (!isValidRequest(Convert.ToInt64(timestamp, 10)))
            {
                error = "请求超时";
                return false;
            }

            if (appid != userA_APPID)
            {
                error = "非法用户";
                return false;
            }

            StringBuilder string1Builder = new StringBuilder();
            string1Builder.Append("appid=").Append(appid).Append("&")
              .Append("timestamp=").Append(timestamp);

            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(HCAMKEY));
            byte[] byteText = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(string1Builder.ToString()));
            string Sign = Convert.ToBase64String(byteText);
            if (Sign.Equals(sign))
            {
                error = string.Empty;
                return true;
            }
            else
            {
                error = "sign无效";
                return false;
            }
        }

        private static string[] strs = new string[]
                         {
                                  "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                         };
        public static string newNoncestr()
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < 15; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        public JObject Get()
        {
            HttpRequest request = HttpContext.Current.Request;
            string appid = request.Params.Get("appid");
            string timestamp = request.Params.Get("timestamp");
            string sign = request.Params.Get("sign");
            string err = string.Empty;

            if (!authenticate(appid, timestamp, sign, out err))
            {
                JObject error = new JObject();
                error.Add("code", -1);
                error.Add("error", err);
                return error;
            }

            JObject ret = new JObject();
            ret.Add("code", 0);
            userA_access_token = newNoncestr();
            userA_expires_timestamp = nowTimeStamp() + expires_in;

            ret.Add("access_token", userA_access_token);
            ret.Add("expires_in", expires_in);
            return ret;
        }
    }
}
