using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web.Helpers;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    public class VkApiRequests : IVkApiRequests
    {
        private const string ApiPattern = "https://api.vk.com/method/{0}?{1}&access_token={2}&v={3}";
        private Func<string> Token;
        private string Ver = "";

        public string SendRequest(string method, Dictionary<string, string> parametrs)
        {
            if (Token == null)
                throw new NullReferenceException();
            var request = string.Format(ApiPattern, method, string.Join("&", parametrs.Select(p => p.Key + "=" + p.Value)), Token(), Ver);
            // Console.WriteLine(request);
            var result = request.GetAsync().Result.Content
                .ReadAsStringAsync().Result;
            // Console.WriteLine(result);
            var jtok = JObject.Parse(result);
            if (jtok.SelectToken("error") != null)
                throw new ArgumentException();
            return jtok.SelectToken("response").ToString();
        }

        public void SetToken(Func<string> token)
        {
            if (Token != null)
                throw new AccessViolationException();
            Token = token;
        }

        public void SetVersion(string ver)
        {
            if (!Ver.Equals(""))
                throw new AccessViolationException();
            Ver = ver;
        }
    }

}
