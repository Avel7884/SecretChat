using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Util;
using Newtonsoft.Json.Linq;
using SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.CustomVkInteraction
{
    public class VkUsersManager : IVkUsersManager
    {
        private static Dictionary<string, string> userById;

        private static Dictionary<string, List<string>> IdsByName; 

        public IVkApiRequests apiRequests;
        private bool friendsWasSelected;

        public VkUsersManager(IVkApiRequests apiRequests)
        {
            this.apiRequests = apiRequests;
            userById = new Dictionary<string, string>();
        }
        
        public string GetNameById(string id)
        {
            if (!userById.ContainsKey(id))
            {
                var result = apiRequests.SendRequest(VkApiCommands.GetUser, new Dictionary<string, string>
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
            if (!friendsWasSelected)
            {
                friendsWasSelected = true;
                var result = apiRequests.SendRequest(VkApiCommands.GetFriends, new Dictionary<string, string>
                {
                    {"fields", "domain"}
                });
                var content = JObject.Parse(result);
                userById.Merge(
                    content["items"]
                        .ToDictionary(t => t.SelectToken("id").ToString(),
                            t => string.Join(" ", 
                                t.SelectToken("first_name").ToString(), 
                                t.SelectToken("last_name").ToString()))
                    );
            }

            return userById.Where(p => p.Value.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(p => $"({p.Value}) -> {p.Key}")
                .ToList();
        }
    }
}
