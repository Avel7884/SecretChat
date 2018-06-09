using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    class VKDialog : IDialog
    {
        private int lastMessageId;
        private string user;
        private string chat;
        private const int chatBias = (int)2e9;

        public VKDialog(string user, string members)
        {
            this.user = user;
            lastMessageId = 0;
            if (!TryToConnectToExistedDialog())
                CreateDialog(members);
        }

        private const string commandGetDialogs = "messages.getDialogs";
        private bool TryToConnectToExistedDialog()
        {
            var content = VKAPIRequests.SendRequest(commandGetDialogs, new Dictionary<string, string>());
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
            chat = (string)VKAPIRequests.SendRequest(
                commandCreateChat,
                new Dictionary<string, string>
                {
                    {"user_ids", members}, 
                    {"title", "6495077 secret chat"}
                });
        }

        private const string commandGetMessage = "messages.get";
        public bool getMessages(out List<string> messages)
        {
            var content = VKAPIRequests.SendRequest(commandGetMessage,
                new Dictionary<string, string>
                {
                    {"last_message_id", lastMessageId.ToString()}
                });
            messages = content["items"]
                            .Where(x => x.SelectToken("chat_id")?.ToString() == chat)
                            .Select(x =>
                            {
                                lastMessageId = Math.Max(lastMessageId, int.Parse(x.SelectToken("id").ToString()));
                                return x.SelectToken("body").ToString();
                            })
                            .Reverse()
                            .ToList();
            return messages.Count != 0;
        }

        private const string commandSendMessage = "messages.send";

        public bool sendMessage(string message)
        {
            JToken content;
            try
            {
                content = VKAPIRequests.SendRequest(commandSendMessage, new Dictionary<string, string>
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
            VKAPIRequests.SendRequest(commandExit, new Dictionary<string, string>
            {
                {"chat_id", chat},
                {"user_id", user}
            });
            var peer = (chatBias + int.Parse(chat)).ToString();
            VKAPIRequests.SendRequest(commandDelete, new Dictionary<string, string>
            {
                {"user_id", user},
                {"peer_id", peer}
            });
        }
    }
}
