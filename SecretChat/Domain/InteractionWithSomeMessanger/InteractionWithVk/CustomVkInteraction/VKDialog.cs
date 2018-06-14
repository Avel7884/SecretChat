using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;
using SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.CustomVkInteraction
{
    class VkDialog: IDialog
    {
        private int lastMessageId;
        private readonly string user;
        private string chat;
        private const int chatBias = (int)2e9;
        private readonly IVkApiRequests apiRequests;
        private readonly IVkUsersManager UsersManager;

        public VkDialog(string user, string members, IVkUsersManager usersManager, IVkApiRequests apiRequests)
        {
            UsersManager = usersManager;
            this.apiRequests = apiRequests;
            this.user = user;
            lastMessageId = 0;
            members += " " + user;
            var membersList = members.Split()
                .OrderBy(m => m)
                .Distinct()
                .ToList();
            if (!TryToConnectToExistedDialog(membersList))
                CreateDialog(membersList);
        }

        
        private bool TryToConnectToExistedDialog(List<string> members)
        {
            var result = apiRequests.SendRequest(VkApiCommands.GetDialogs, new Dictionary<string, string>());
            var content = JObject.Parse(result);
            var idToken = content.SelectToken("items")
                ?.Select(x => x["message"])
                .Where(x => x.SelectToken("title")
                    .ToString()
                    .StartsWith("6495077"))
                ?.FirstOrDefault(x => HasEqualsMembers(x.SelectToken("chat_id").ToString(), members));
            
            if (idToken == null) 
                return false;
            chat = idToken["chat_id"].ToString();
            return true;
        }

        private bool HasEqualsMembers(string chatId, List<string> members)
        {
            var result = apiRequests.SendRequest(VkApiCommands.GetChatUsers, new Dictionary<string, string>
            {
                {"chat_id", chatId}
            });
            var content = JToken.Parse(result);
            var chatMembers = content.Values()
                .Select(x => x.ToString())
                .OrderBy(x => x)
                .Distinct()
                .ToList();
            return members.SequenceEqual(chatMembers);
        }

        private void CreateDialog(IEnumerable<string> members)
        {
            chat = apiRequests.SendRequest(
                VkApiCommands.CreateChat,
                new Dictionary<string, string>
                {
                    {"user_ids", string.Join(",", members)}, 
                    {"title", "6495077 secret chat"}
                });
        }

        public bool getMessages(out List<Message> messages)
        {
            var result = apiRequests.SendRequest(VkApiCommands.GetMessages,
                new Dictionary<string, string>
                {
                    {"last_message_id", lastMessageId.ToString()}
                });
            var content = JObject.Parse(result);
            messages = content["items"]
                            .Select(x =>
                            {
                                lastMessageId = Math.Max(lastMessageId, int.Parse(x.SelectToken("id").ToString()));
                                return x;
                            })
                            .Where(x => x.SelectToken("chat_id")?.ToString() == chat)
                            .Select(x => new Message(
                                x.SelectToken("body").ToString(), 
                                UsersManager.GetNameById(x.SelectToken("user_id").ToString())
                                ))
                            .Reverse()
                            .ToList();
            return messages.Count != 0;
        }

        public bool sendMessage(IMessage message)
        {
            try
            {
                apiRequests.SendRequest(VkApiCommands.SendMessage, new Dictionary<string, string>
                {
                    {"chat_id", chat},
                    {"message", message.ToString()}
                });
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            apiRequests.SendRequest(VkApiCommands.RemoveChat, new Dictionary<string, string>
            {
                {"chat_id", chat},
                {"user_id", user}
            });
            var peer = (chatBias + int.Parse(chat)).ToString();
            apiRequests.SendRequest(VkApiCommands.DeleteDialog, new Dictionary<string, string>
            {
                {"user_id", user},
                {"peer_id", peer}
            });
        }
    }
}
