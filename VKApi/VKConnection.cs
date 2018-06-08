using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using System.Timers;
using Newtonsoft.Json;

namespace VKApi
{
    class VKConnection : IConnection<VKDialog>
    {
        private Timer timer;
        private const string autrizeRequest = "https://oauth.vk.com/authorize?client_id={0}&display=popup&scope=4098&response_type=token&v={1}";

        private Regex rx = new Regex(@"https://oauth.vk.com/blank.html#access_token=([0-9a-f]+)&expires_in=(\d+)&user_id=(\d+)",
            RegexOptions.Compiled);

        private string secret;
        private string id;
        private string ver;
        private string user;

        public VKConnection(string clientID, string userID, string secretCode, string ver = "5.78")
        {
            secret = secretCode;
            id = clientID;
            user = userID;
            this.ver = ver;
            InitTimer();
        }

        private void InitTimer()
        {
            timer = new Timer() {AutoReset = true};
            timer.Elapsed += (s, e) =>
                InitToken();
        }

        public string Token { get ; private set ; }

        public void Connect()=>
            InitToken();

        public VKDialog StartDialog(string[] ids)
        {
            var dialogMembers = new StringBuilder(user);
            foreach(var id in ids)
            {
                dialogMembers.Append(',');
                dialogMembers.Append(id);
            }
            return new VKDialog(()=>Token, user, dialogMembers.ToString(), ver);
        }

        public Dictionary<string, string> GetFriends()
        {
            throw new NotImplementedException();
        }

        private void InitToken()
        {
            timer.Stop();
            System.Diagnostics.Process.Start(String.Format(autrizeRequest, id, ver));
            Console.WriteLine("Write link:");
            ReadLink();
            timer.Start();
        }

        private void ReadLink()
        {
            var link = Console.ReadLine();
            var groups = rx.Match(link).Groups;
            Token = groups[1].Value;
            user = groups[3].Value;
            timer.Interval = 1000 * int.Parse(groups[2].Value);
        }
    }
}
