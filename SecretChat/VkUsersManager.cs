using System;
using System.Collections.Generic;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    public class VKUsersManager : IUsersManager
    {
        private readonly Dictionary<string, string> userById;

        public VKUsersManager()
        {
            userById = new Dictionary<string, string>();
        }
        
        private const string commandGetUser = "users.get";
        
        public string GetNameById(string id)
        {
            if (!userById.ContainsKey(id))
            {
                var content = VKAPIRequests.SendRequest(commandGetUser, new Dictionary<string, string> {{"user_id", id}});
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