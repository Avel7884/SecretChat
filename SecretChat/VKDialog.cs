using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace SecretChat
{
    class VKDialog : IDialog
    {
        private Func<string> token;
        private string ver;
        private string lastMessageId;
        private string user;
        private string chat;
        private const int chatBias = 2000000000;

        public VKDialog(Func<string> token, string user, string members, string ver)
        {
            this.token = token;
            this.ver = ver;
            this.user = user;
            lastMessageId = "0";
            if (!TryToConnectToExistedDialog())
                CreateDialog(members);
        }

        private const string commandCheck = "https://api.vk.com/method/messages.getDialogs?access_token={0}&v={1}";
        private bool TryToConnectToExistedDialog()
        {
            var res = String.Format(commandCheck, token(),ver).
                                    GetAsync().Result.Content.
                                    ReadAsStringAsync().Result;
            var idToken = JObject.Parse(res)["response"]["items"] //need to check on valid members
                            .Select(x => x["message"])
                            .FirstOrDefault(x => x.SelectToken("title")
                                                    .ToString()
                                                    .StartsWith("6495077"));
            
            if (idToken == null) 
                return false;
            chat = idToken["chat_id"].ToString();
            return true;
        }

        private const string commandCreateChat = "https://api.vk.com/method/messages.createChat?user_ids={0}&title=chat&v={1}&access_token={2}";

        private void CreateDialog(string members)
        {
            var res = string.Format(commandCreateChat, members, ver, token()).GetAsync().Result.Content.ReadAsStringAsync().Result;
            chat = (string)JObject.Parse(res).GetValue("response"); //JsonConvert.DeserializeObject<Dictionary<string, string>>(res)[];
        }

        public bool hasCallback => false;
        public event EventHandler Callback;

        private const string commandGet = "https://api.vk.com/method/messages.get?access_token={0}&v={1}";
        public bool getMessages(out List<string> messages)
        {
            var res = string.Format(commandGet, token(), ver).GetAsync().Result.Content.ReadAsStringAsync().Result;
            messages = JObject.Parse(res)["response"]["items"]
                            .Where(x => x.SelectToken("chat_id")?.ToString() == chat)
                            .Select(x => x["body"].ToString())
                            .Reverse()
                            .ToList();
            return messages.Count != 0;
        }

        private const string commandSend = "https://api.vk.com/method/messages.send?chat_id={0}&message={1}&access_token={2}&v={3}";

        public bool sendMessage(string message)
        {
            return string.Format(commandSend, chat, message, token(), ver).GetAsync().Result.IsSuccessStatusCode;
        }

        private const string commandExit = "https://api.vk.com/method/messages.removeChatUser?chat_id={0}&user_id={1}&access_token={2}&v={3}";
        private const string commandDelete = "https://api.vk.com/method/messages.deleteDialog?user_id={0}&peer_id={1}&access_token={2}&v={3}";
        public void Dispose()
        {
            string.Format(commandExit, chat, user, token, ver).GetAsync();
            var peer = (chatBias + int.Parse(chat)).ToString();
            string.Format(commandDelete, user, peer, token, ver).GetAsync();
        }
    }
}
