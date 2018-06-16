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

        public readonly IVkApiRequests ApiRequests;
        private bool friendsWasSelected;

        public VkUsersManager(IVkApiRequests apiRequests)
        {
            friendsWasSelected = false;
            ApiRequests = apiRequests;
            userById = new Dictionary<string, string>();
        }
        
        public string GetNameById(string id)
        {
            if (!userById.ContainsKey(id))
            {
                var result = ApiRequests.SendRequest(VkApiCommands.GetUser, new Dictionary<string, string>
                {
                    {"user_id", id}
                });
                var content = JToken.Parse(result)[0];
                var name = string.Join(" ", 
                    content.SelectToken("first_name").ToString(), 
                    content.SelectToken("last_name").ToString());
                userById.Add(id, name);
            }

            return userById[id];
        }
    
        public IEnumerable<string> GetIdsByName(string name)
        {
            if (!friendsWasSelected)
            {
                friendsWasSelected = true;
                SelectFriends();
            }

            return userById.Where(p => p.Value.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(p => $"({p.Value}) -> {p.Key}");
        }

        private void SelectFriends()
        {
            var result = ApiRequests.SendRequest(VkApiCommands.GetFriends, new Dictionary<string, string>
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
    }
}
