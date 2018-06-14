using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.CustomVkInteraction
{
    public class VkApiRequests : IVkApiRequests
    {
        private const string ApiPattern = "https://api.vk.com/method/{0}?{1}&access_token={2}&v={3}";
        private Func<string> token;
        private string ver = "";

        public string SendRequest(string method, Dictionary<string, string> parametrs)
        {
            if (token == null)
                throw new NullReferenceException();
            var request = string.Format(ApiPattern, method, string.Join("&", parametrs.Select(p => p.Key + "=" + p.Value)), token(), ver);
            var result = request.GetAsync().Result.Content
                .ReadAsStringAsync().Result;
            var jtok = JObject.Parse(result);
            if (jtok.SelectToken("error") != null)
                throw new ArgumentException();
            return jtok.SelectToken("response").ToString();
        }

        public void SetToken(Func<string> token)
        {
            if (this.token != null)
                throw new AccessViolationException();
            this.token = token;
        }

        public void SetVersion(string ver)
        {
            if (!this.ver.Equals(""))
                throw new AccessViolationException();
            this.ver = ver;
        }
    }

}
