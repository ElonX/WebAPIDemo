using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebServerDemo.Controllers
{
    public class AuthenticateUtil
    {
        static string HCAMKEY = "AABBCCDD";
        public static int token_expires_in = 60;//有效期60秒
        static int request_expires_in = 10;//有效期10秒

        private static string[] strs = new string[]
                 {
                                  "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                 };
        public static string newToken()
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < 30; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        public static long nowTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static bool isValidRequest(long timestamp)//请求有效期+5-5分钟
        {
            long nowts = nowTimeStamp();
            return (nowts + request_expires_in >= timestamp) && (nowts - timestamp < request_expires_in);
        }

        public static string HMAC(string data)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(HCAMKEY));
            byte[] byteText = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(byteText);
        }
    }
}