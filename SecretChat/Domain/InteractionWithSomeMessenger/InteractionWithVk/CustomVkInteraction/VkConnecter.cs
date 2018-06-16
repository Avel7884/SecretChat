using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;
using SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction;
using SecretChat.Infrastructure;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.CustomVkInteraction
{
    class VkConnecter: IConnecter<VkDialog>
    {
        private Timer timer;
        const string writeLinkMessage = "Write link:";
        const string helpMessage = "Write users id separated by space\nIf you want to get id of users in your friend list, write '? <User Name>'";
        private const string autrizeRequest = "https://oauth.vk.com/authorize?client_id={0}&display=popup&scope=4098&response_type=token&v={1}";

        private static readonly Regex rx = new Regex(@".*access_token=([0-9a-f]+)&expires_in=(\d+)&user_id=(\d+)",
            RegexOptions.Compiled);

        private readonly IInteracter interacter;
        private string applicationClientId;
        private string ver;
        private string user;
        private IVkUsersManager usersManager;
        private IVkApiRequests apiRequests;
        private string Token { get ; set ; }

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
            timer = new Timer {AutoReset = true};
            timer.Elapsed += (s, e) =>
                InitToken();
        }

        public void Connect()
        {
            InitToken();
            apiRequests.Token = () => Token;
            apiRequests.Ver = ver;
            interacter.WriteLine($"Welcome, {usersManager.GetNameById(user)}!");
        }

        public VkDialog StartDialog()
        {
            return new VkDialog(user, GetDialogUsers(), usersManager, apiRequests);
        }

        private void InitToken()
        {
            timer.Stop();
            System.Diagnostics.Process.Start(
                string.Format(autrizeRequest, applicationClientId, ver));
            interacter.WriteLine(writeLinkMessage);
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
        
        private string GetDialogUsers()
        {
            interacter.WriteLine(helpMessage);
            var users = interacter.ReadLine();
            while (users.StartsWith("?"))
            {
                interacter.WriteLine(string.Join("\n", 
                    usersManager.GetIdsByName(users.Substring(2))));
                users = interacter.ReadLine();
            }
            return users;
        }
    }
}
