using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Helpers;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    public static class VKAPIRequests
    {
        private const string apiPattern = "https://api.vk.com/method/{0}?{1}&access_token={2}&v={3}";
        internal static Func<string> Token;
        internal static string Ver;

        public static JToken SendRequest(string method, Dictionary<string, string> parametrs)
        {
            var request = string.Format(apiPattern, method, string.Join("&", parametrs.Select(p => p.Key + "=" + p.Value)), Token(), Ver);
            var result = request.GetAsync().Result.Content
                .ReadAsStringAsync().Result;
            Console.WriteLine(request);
            Console.WriteLine(result);
            var jtok = JObject.Parse(result);
            if (jtok.SelectToken("error") != null)
                throw new ArgumentException();
            return jtok.SelectToken("response");
        }
    }
}