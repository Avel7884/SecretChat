using System;
using System.Collections;
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
    class VkConnecter : IConnecter<VKDialog>
    {
        private Timer timer;
        private const string autrizeRequest = "https://oauth.vk.com/authorize?client_id={0}&display=popup&scope=4098&response_type=token&v={1}";

        private static readonly Regex rx = new Regex(@"https://oauth.vk.com/blank.html#access_token=([0-9a-f]+)&expires_in=(\d+)&user_id=(\d+)",
            RegexOptions.Compiled);

        private readonly IInteracter interactor;
        private string id;
        private string ver;
        private string user;

        public VkConnecter(string clientID, IInteracter interactor, string ver = "5.78")
        {
            this.interactor = interactor;
            id = clientID;
            this.ver = ver;
            InitTimer();
        }

        private void InitTimer()
        {
            timer = new Timer() {AutoReset = true};
            timer.Elapsed += (s, e) =>
                InitToken();
        }

        private string Token { get ; set ; }

        public void Connect()=>
            InitToken();

        public VKDialog StartDialog(IEnumerable ids)
        {
            return new VKDialog(() => Token, user, string.Join(",", ids), ver);
        }

        public Dictionary<string, string> GetFriends()
        {
            throw new NotImplementedException();
        }

        private void InitToken()
        {
            timer.Stop();
            System.Diagnostics.Process.Start(String.Format(autrizeRequest, id, ver));
            interactor.WriteLine("Write link:");
            ReadLink();
            timer.Start();
        }

        private void ReadLink()
        {
            var link = interactor.ReadLine();
            var groups = rx.Match(link).Groups;
            Token = groups[1].Value;
            user = groups[3].Value;
            timer.Interval = int.Parse(groups[2].Value) * 1000;
        }
    }
}
