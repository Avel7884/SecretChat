using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.CustomVkInteraction
{
    public class VkApiRequests : IVkApiRequests
    {
        private const string ApiPattern = "https://api.vk.com/method/{0}?{1}&access_token={2}&v={3}";
        private Func<string> token;
        private string ver;

        public string SendRequest(string method, Dictionary<string, string> parametrs)
        {
            if (Token == null)
                throw new NullReferenceException();
            var request = string.Format(ApiPattern, method, string.Join("&", parametrs.Select(p => p.Key + "=" + p.Value)), Token(), Ver);
            var result = request.GetAsync().Result.Content
                .ReadAsStringAsync().Result;
            var jtok = JObject.Parse(result);
            if (jtok.SelectToken("error") != null)
            {
                throw new ArgumentException();
            }

            return jtok.SelectToken("response").ToString();
        }

        public Func<string> Token
        {
            private get => token;
            set
            {
                if (token != null)
                    throw new AccessViolationException();
                token = value;
            }
        }

        public string Ver
        {
            private get => ver;
            set
            {
                if (ver != null)
                    throw new AccessViolationException();
                ver = value;
            }
        }
    }

}
