using System;
using System.Collections.Generic;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    public class VKUsersManager : IVkUsersManager
    {
        private static Dictionary<string, string> userById;
        public IVkApiRequests apiRequests;

        public VKUsersManager(IVkApiRequests apiRequests)
        {
            this.apiRequests = apiRequests;
            userById = new Dictionary<string, string>();
        }
        
        private const string commandGetUser = "users.get";
        
        public string GetNameById(string id)
        {
            if (!userById.ContainsKey(id))
            {
                var result = apiRequests.SendRequest(commandGetUser, new Dictionary<string, string>
                {
                    {"user_id", id}
                });
                // Console.WriteLine(result);
                var content = JToken.Parse(result)[0];
                var name = string.Join(" ", 
                    content.SelectToken("first_name").ToString(), 
                    content.SelectToken("last_name").ToString());
                userById.Add(id, name);
            }

            return userById[id];
        }

        public List<string> GetIdsByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}