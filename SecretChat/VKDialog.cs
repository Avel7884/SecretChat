using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    class VkDialog: IDialog
    {
        private int lastMessageId;
        private string user;
        private string chat;
        private const int chatBias = (int)2e9;
        private IVkApiRequests apiRequests;
        public IVkUsersManager UsersManager;

        public VkDialog(string user, string members, IVkUsersManager usersManager, IVkApiRequests apiRequests)
        {
            UsersManager = usersManager;
            this.apiRequests = apiRequests;
            this.user = user;
            lastMessageId = 0;
            if (!TryToConnectToExistedDialog())
                CreateDialog(members);
        }

        private const string commandGetDialogs = "messages.getDialogs";
        private bool TryToConnectToExistedDialog()
        {
            var result = apiRequests.SendRequest(commandGetDialogs, new Dictionary<string, string>());
            var content = JObject.Parse(result);
            var idToken = content.SelectToken("items") //need to check on valid members
                            ?.Select(x => x["message"])
                             .FirstOrDefault(x => x.SelectToken("title")
                                                    .ToString()
                                                    .StartsWith("6495077"));
            
            if (idToken == null) 
                return false;
            chat = idToken["chat_id"].ToString();
            return true;
        }

        private const string commandCreateChat = "messages.createChat";

        private void CreateDialog(string members)
        {
            chat = apiRequests.SendRequest(
                commandCreateChat,
                new Dictionary<string, string>
                {
                    {"user_ids", members}, 
                    {"title", "6495077 secret chat"}
                });
        }

        private const string commandGetMessage = "messages.get";
        public bool getMessages(out List<Message> messages)
        {
            var result = apiRequests.SendRequest(commandGetMessage,
                new Dictionary<string, string>
                {
                    {"last_message_id", lastMessageId.ToString()}
                });
            var content = JObject.Parse(result);
            messages = content["items"]
                            .Where(x => x.SelectToken("chat_id")?.ToString() == chat)
                            .Select(x =>
                            {
                                lastMessageId = Math.Max(lastMessageId, int.Parse(x.SelectToken("id").ToString()));
                                return new Message(
                                    x.SelectToken("body").ToString(), 
                                    UsersManager.GetNameById(x.SelectToken("user_id").ToString())
                                    );
                            })
                            .Reverse()
                            .ToList();
            return messages.Count != 0;
        }

        private const string commandSendMessage = "messages.send";

        public bool sendMessage(string message)
        {
            try
            {
                apiRequests.SendRequest(commandSendMessage, new Dictionary<string, string>
                {
                    {"chat_id", chat},
                    {"message", message}
                });
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        private const string commandExit = "messages.removeChatUser";
        private const string commandDelete = "messages.deleteDialog?user_id={0}&peer_id={1}&access_token={2}&v={3}";
        public void Dispose()
        {
            apiRequests.SendRequest(commandExit, new Dictionary<string, string>
            {
                {"chat_id", chat},
                {"user_id", user}
            });
            var peer = (chatBias + int.Parse(chat)).ToString();
            apiRequests.SendRequest(commandDelete, new Dictionary<string, string>
            {
                {"user_id", user},
                {"peer_id", peer}
            });
        }
    }
}
