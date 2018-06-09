using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;

namespace SecretChat
{
    class VkConnecter: IConnecter<VkDialog>
    {
        private Timer timer;
        private const string autrizeRequest = "https://oauth.vk.com/authorize?client_id={0}&display=popup&scope=4098&response_type=token&v={1}";

        private static readonly Regex rx = new Regex(@"https://oauth.vk.com/blank.html#access_token=([0-9a-f]+)&expires_in=(\d+)&user_id=(\d+)",
            RegexOptions.Compiled);

        private readonly IInteracter interacter;
        private string applicationClientId;
        private string ver;
        private string user;
        private IVkUsersManager usersManager;
        private IVkApiRequests apiRequests;

        public VkConnecter(string applicationClientId, 
            IInteracter interacter, 
            IVkUsersManager usersManager, 
            IVkApiRequests apiRequests, 
            string ver = "5.78")
        {
            this.usersManager = usersManager;
            this.apiRequests = apiRequests;
            this.interacter = interacter;
            this.applicationClientId = applicationClientId;
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

        public void Connect()
        {
            InitToken();
            apiRequests.SetToken(() => Token);
            apiRequests.SetVersion(ver);
            interacter.WriteLine($"Welcome, {usersManager.GetNameById(user)}!");
        }

        public VkDialog StartDialog(IEnumerable ids)
        {
            return new VkDialog(user, string.Join(",", ids), usersManager, apiRequests);
        }

        public Dictionary<string, string> GetFriends()
        {
            throw new NotImplementedException();
        }

        private void InitToken()
        {
            timer.Stop();
            System.Diagnostics.Process.Start(string.Format(autrizeRequest, applicationClientId, ver));
            interacter.WriteLine("Write link:");
            ReadLink();
            timer.Start();
        }

        private void ReadLink()
        {
            var link = interacter.ReadLine();
            var groups = rx.Match(link).Groups;
            Token = groups[1].Value;
            user = groups[3].Value;
            timer.Interval = int.Parse(groups[2].Value) * 1000;
        }
    }
}
