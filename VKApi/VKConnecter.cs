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
    class VKConnecter : IConnecter<VKDialog>
    {
        private Timer timer;
        private const string autrizeRequest = "https://oauth.vk.com/authorize?client_id={0}&display=popup&scope=4098&response_type=token&v={1}";

        private Regex rx = new Regex(@"https://oauth.vk.com/blank.html#access_token=([0-9a-f]+)&expires_in=(\d+)&user_id=(\d+)",
            RegexOptions.Compiled);

        private string id;
        private string ver;
        private string token;
        private string user;

        public VKConnecter(string clientID, string ver = "5.78")
        {
            id = clientID;
            this.ver = ver;
        }

        public string Token { get => token; private set => token = value; }

        public VKDialog Connect()
        {
            getToken();
            return new VKDialog(()=>Token, id, user, ver);
        }

        private void getToken()
        {
            System.Diagnostics.Process.Start(String.Format(autrizeRequest, id, ver));
            Console.WriteLine("Write link:");
            var link = Console.ReadLine();
            var groups = rx.Match(link).Groups;
            Token = groups[1].Value;
            user = groups[3].Value;
            SetTimer(int.Parse(groups[2].Value));
        }

        private void SetTimer(int interval)
        {
            timer = new Timer(interval) {AutoReset = true};
            timer.Elapsed += (s, e) => 
            {
                timer.Stop();
                getToken();
                timer.Start();
            };
        }

        public string GetName(string id)
        {
            return "Joe Doe";
        }
    }
}
