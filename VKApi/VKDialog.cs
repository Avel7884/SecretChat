using Flurl.Http;
using Newtonsoft.Json;
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
        //private string id;
        private string ver;
        private string last_message_id;
        private string user;
        private string chat;

        private string commandCreateChat = "https://api.vk.com/method/messages.createChat?user_ids={0}&title=chat&v={1}&access_token={2}";
        public VKDialog(Func<string> token, string user, string members, string ver)
        {
            this.token = token;
            //this.id = id;
            this.ver = ver;
            last_message_id = "0";
            //this.user = user; + ',' + Console.ReadLine()
            var res = String.Format(commandCreateChat, members, ver, token()).GetAsync().Result.Content.ReadAsStringAsync().Result;
            
            //if (!res.IsSuccessStatusCode)
            //   throw new Exception(res.Content.ReadAsStringAsync().Result);
            //chat = res.Split(':')[2].Split('}')[0];
            chat = JsonConvert.DeserializeObject<Dictionary<string, string>>(res)["response"];
            Console.WriteLine(chat);
        }

        public bool hasCallback => false;
        //It appears, Callbacks doesn`t works for chats.(
        public event EventHandler Callback;

        private string commandGet = "https://api.vk.com/method/messages.getDialogs?chat_id={0}&access_token={1}&start_message_id={2}&v={3}";
        public bool getMessages(out string[] messages)
        {
            var resp = String.Format(commandGet, chat, token(), last_message_id, ver).GetAsync().Result.Content.ReadAsStringAsync().Result;
            //if (!resp.IsSuccessStatusCode)
            //    throw new Exception(resp.Content.ReadAsStringAsync().Result);
            //foreach(var res in JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.DeserializeObject<Dictionary<string, string>>(resp)["respond"]))
            messages = new[] {resp};
            return true;//resp.IsSuccessStatusCode; // TODO learn to parse
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
            var peer = (2000000000 + int.Parse(chat)).ToString();
            String.Format(commandDelete, user, peer, token, ver).GetAsync();
        }
    }
}
