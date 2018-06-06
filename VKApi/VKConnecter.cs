using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using System.Timers;

namespace VKApi
{
    class VKConnecter : IConnecter<VKDialog>
    {
        private Timer timer;
        private const string autrizeRequest = "https://oauth.vk.com/authorize?client_id={0}&display=page&scope=4098&response_type=code&v={1}";
        private const string tokenRequest = "https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&code={2}";

        private string secret;
        private string id;
        private string ver;
        private string token;
        private string user;

        public VKConnecter(string clientID, string secretCode,string userID, string ver = "5.78")
        {
            secret = secretCode;
            id = clientID;
            this.ver = ver;
            this.user = userID;
        }

        public string Token { get => token; private set => token = value; }

        public VKDialog Connect()
        {
            timer = new Timer(24 * 60 * 60);
            timer.AutoReset = true;
            timer.Elapsed += (s, e) => 
            {
                timer.Stop();
                Token = getToken();
                timer.Start();
            };
            token = getToken();
            return new VKDialog(()=>token, id,user, ver);
        }

        private string getToken()
        {
            System.Diagnostics.Process.Start(String.Format(autrizeRequest,id,ver));
            Console.WriteLine("Write your code:");
            var code = Console.ReadLine();
            var res = String.Format(tokenRequest, id , secret, code).GetAsync().Result;//TODO Normal parsing
            return res.Content.ReadAsStringAsync().Result.Split(new[] { '"' }, 5)[3];
        }
    }
}
