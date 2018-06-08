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
        private const string tokenRequest = "https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&code={2}";

        private Regex rx = new Regex(@"https://oauth.vk.com/blank.html#access_token=([0-9a-f]+)&expires_in=(\d+)&user_id=(\d+)",
            RegexOptions.Compiled);

        private string secret;
        private string id;
        private string ver;
        private string token;
        private string user;

        public VKConnecter(string clientID, string secretCode, string ver = "5.78")
        {
            secret = secretCode;
            id = clientID;
            this.ver = ver;
        }

        public string Token { get => token; private set => token = value; }

        public VKDialog Connect()
        {
            timer = new Timer(24 * 60 * 60);
            timer.AutoReset = true;
            timer.Elapsed += (s, e) => 
            {
                timer.Stop();
//                Token = getToken();
                timer.Start();
            };
            getToken();
            return new VKDialog(()=>token, id,user, ver);
        }

        private void getToken()
        {
            System.Diagnostics.Process.Start(String.Format(autrizeRequest, id, ver));
            Console.WriteLine("Write link:");
            var link = Console.ReadLine();
            Token = rx.Match(link).Groups[1].Value;
            user = rx.Match(link).Groups[3].Value;
        }
    }
}
