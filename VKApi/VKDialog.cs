﻿﻿using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VKApi
{
    class VKDialog : IDialog
    {
        private Func<string> token;
        private string ver;
        private string last_message_id;
        private string user;
        private string chat;
        private const int chatBias = 2000000000;

        public VKDialog(Func<string> token, string user, string members, string ver)
        {
            this.token = token;
            this.ver = ver;
            this.user = user;
            last_message_id = "0";
            if (!TryToConnectToExistedDialog())
                CreateDialog(members);
        }

        private string commandCheck = "https://api.vk.com/method/messages.getDialogs?access_token={0}&v={1}";
        private bool TryToConnectToExistedDialog()
        {
            var res = String.Format(commandCheck, token(),ver).
                                    GetAsync().Result.Content.
                                    ReadAsStringAsync().Result;
            var idToken = JObject.Parse(res)["response"]["items"] //need to check on valid members
                            .Select(x => x["message"])
                            .FirstOrDefault(x => x["title"] != null &&
                                        x["title"].ToString() == "6495077 Secret chat");
            if (idToken != null)
            {
                chat = idToken["chat_id"].ToString();
                return true;
            }
            else
                return false;
        }

        private string commandCreateChat = "https://api.vk.com/method/messages.createChat?user_ids={0}&title=chat&v={1}&access_token={2}";
        private void CreateDialog(string members)
        {
            var res = String.Format(commandCreateChat, members, ver, token()).GetAsync().Result.Content.ReadAsStringAsync().Result;
            chat = (string)JObject.Parse(res).GetValue("response"); //JsonConvert.DeserializeObject<Dictionary<string, string>>(res)[];
        }

        public bool hasCallback => false;
        public event EventHandler Callback;

        private string commandGet = "https://api.vk.com/method/messages.get?access_token={0}&v={1}";
        public bool getMessages(out string[] messages)
        {
            var res = String.Format(commandGet, token(), ver).GetAsync().Result.Content.ReadAsStringAsync().Result;
            messages = JObject.Parse(res)["response"]["items"]
                            .Where(x => x["chat_id"].ToString() == chat)
                            .Select(x => x["body"].ToString())
                            .ToArray();
            return true;
        }

        private string commandSend = "https://api.vk.com/method/messages.send?chat_id={0}&message={1}&access_token={2}&v={3}";
        public bool sendMessage(string message)
        {
            return String.Format(commandSend, chat, message, token(), ver).GetAsync().Result.IsSuccessStatusCode;
        }

        private string commandExit = "https://api.vk.com/method/messages.removeChatUser?chat_id={0}&user_id={1}&access_token={2}&v={3}";
        private string commandDelete = "https://api.vk.com/method/messages.deleteDialog?user_id={0}&peer_id={1}&access_token={2}&v={3}";
        public void Dispose()
        {
            String.Format(commandExit, chat, user, token, ver).GetAsync();
            var peer = (chatBias + int.Parse(chat)).ToString();
            String.Format(commandDelete, user, peer, token, ver).GetAsync();
        }
    }
}
