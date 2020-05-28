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

        private bool authenticate(string appid, string timestamp, string sign, out string error)
        {
            if (!AuthenticateUtil.isValidRequest(Convert.ToInt64(timestamp, 10)))
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

            string Sign = AuthenticateUtil.HMAC(string1Builder.ToString());
            if (!Sign.Equals(sign))
            {
                error = "sign无效";
                return false;
            }

            error = string.Empty;
            return true;
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
            userA_access_token = AuthenticateUtil.newToken();
            userA_expires_timestamp = AuthenticateUtil.nowTimeStamp() + AuthenticateUtil.token_expires_in;

            ret.Add("access_token", userA_access_token);
            ret.Add("expires_in", AuthenticateUtil.token_expires_in);
            return ret;
        }
    }
}
