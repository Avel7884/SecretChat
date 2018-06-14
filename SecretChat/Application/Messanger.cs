using System;

namespace SecretChat
{
    public class Messanger : IMessanger
    {
        private IConnecter<IDialog> connecter;
        private MessageStream messageStream;
        private IInteracter interacter;
        private IDialog dialog;
        private IUsersManager usersManager;

        public Messanger(IConnecter<IDialog> connecter, MessageStream messageStream, 
            IUsersManager usersManager, IInteracter interacter)
        {
            this.connecter = connecter;
            this.messageStream = messageStream;
            this.usersManager = usersManager;
            this.interacter = interacter;
        }

        public void LogIn()
        {
            connecter.Connect();
        }

        public void CreateChat()
        {
            interacter.WriteLine("Write users id separated by space\n" +
                                 "If you want to get id of users in your friend list, write '? <User Name>'");
            var users = interacter.ReadLine();
            while (users.StartsWith("?"))
            {
                interacter.WriteLine(string.Join("\n", usersManager.GetIdsByName(users.Split()[1])));
                users = interacter.ReadLine();
            }
            dialog = connecter.StartDialog(users);
            interacter.WriteLine("Connection is successful! You can start messaging!");
        }

        public void SendMessage()
        {
            var message = messageStream.ReadMessage();
            dialog.sendMessage(message);
        }

        public void GetMessages()
        {
            if (!dialog.getMessages(out var messages)) return;
            foreach (var message in messages)
            {
                messageStream.WriteMessage($"({message.Sender}) > ", message.Content);
            }
        }
    }
}
