using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace WebServerDemo.Controllers
{
    public class test01Controller : ApiController
    {
        string HCAMKEY = "AABBCCDD";

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

        private bool authenticate(string nonce, string timestamp, string sign, out string error)
        {
            if (!isValidRequest(Convert.ToInt64(timestamp, 10)))
            {
                error = "请求超时";
                return false;
            }

            StringBuilder string1Builder = new StringBuilder();
            string1Builder.Append("nonce=").Append(nonce).Append("&")
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

        public JObject Post()
        {
            HttpRequest request = HttpContext.Current.Request;
            string nonce = request.Params.Get("nonce");
            string timestamp = request.Params.Get("timestamp");
            string sign = request.Params.Get("sign");
            string err = string.Empty;

            if (!authenticate(nonce, timestamp, sign, out err))
            {
                JObject error = new JObject();
                error.Add("error", err);
                return error;
            }

            System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] bs = new byte[s.Length];
            s.Read(bs, 0, (int)s.Length);
            string data = Encoding.UTF8.GetString(bs);
            JObject jdata = JObject.Parse(data);
            int a = (int)jdata["a"];
            int b = (int)jdata["b"];

            JObject ret = new JObject();
            ret.Add("result", a + b);
            return ret;
        }
    }
}
