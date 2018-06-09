using System;
using System.Collections.Generic;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    public class VKUsersManager : IUsersManager
    {
        private readonly Dictionary<string, string> userById;
        private Func<string> token;
        private string ver;

        public VKUsersManager(Func<string> token, string ver)
        {
            this.token = token;
            this.ver = ver;
            userById = new Dictionary<string, string>();
        }
        
        private const string commandGetUser = "https://api.vk.com/method/users.get?user_id={0}&access_token={1}&v={2}";
        
        public string GetNameById(string id)
        {
            if (!userById.ContainsKey(id))
            {
                var res = string.Format(commandGetUser, id, token, ver)
                    .GetAsync().Result.Content.
                    ReadAsStringAsync().Result;;
                var content = JObject.Parse(res)["response"];
                var name = content.SelectToken("first_name") + content.SelectToken("last_name").ToString();
                userById.Add(id, name);
            }

            return userById[id];
        }

        public List<string> GetIdsByUser(string name)
        {
            throw new NotImplementedException();
        }
    }
}