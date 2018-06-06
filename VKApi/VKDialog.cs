using Flurl.Http;
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
        private string id;
        private string ver;
        //private string user;
        private string chat;

        private string commandCreateChat = "https://api.vk.com/method/messages.createChat?user_ids={0}&title=chat&v={1}&access_token={2}";
        public VKDialog(Func<string> token, string id, string ver, string user)
        {
            this.token = token;
            this.id = id;
            this.ver = ver;
            //this.user = user; + ',' + Console.ReadLine()
            var res = String.Format(commandCreateChat, user, ver, token()).GetAsync().Result.Content.ReadAsStringAsync().Result;

            Console.WriteLine(res);
            //if (!res.IsSuccessStatusCode)
            //   throw new Exception(res.Content.ReadAsStringAsync().Result);
            chat = res.Split(':')[2].Split('}')[0];
            Console.WriteLine(chat);
        }

        public bool hasCallback => false;
        //It appears, Callbacks doesn`t works for chats.(
        public event EventHandler Callback;

        private string commandGet = "https://api.vk.com/method/messages.get?chat_id={0}&access_token={1}&last_message_id={2}&v={3}";
        public bool getMessages(out string[] messages)
        {
            var resp = String.Format(commandGet, chat, token(), 0, ver).GetAsync().Result.Content.ReadAsStringAsync().Result;
            //if (!resp.IsSuccessStatusCode)
            //    throw new Exception(resp.Content.ReadAsStringAsync().Result);
            messages = new[] { resp};
            return true;//resp.IsSuccessStatusCode; // TODO learn to parse
        }

        private string commandSend = "https://api.vk.com/method/messages.send?chat_id={0}&message={1}&access_token={2}&v={3}";
        public bool sendMessage(string message)
        {
            return String.Format(commandSend, chat, message, token(), ver).GetAsync().Result.IsSuccessStatusCode;
        }

        public void Dispose()
        {
            //TODO removing from chat
        }
    }
}
