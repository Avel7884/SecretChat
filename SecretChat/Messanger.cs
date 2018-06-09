using System;

namespace SecretChat
{
    public class Messanger : IMessanger
    {
        private IConnecter<IDialog> connecter;
        private IMessageStream messageStream;
        private IInteracter interacter;
        private IDialog dialog;

        public Messanger(IConnecter<IDialog> connecter, IMessageStream messageStream, IInteracter interacter)
        {
            this.connecter = connecter;
            this.messageStream = messageStream;
            this.interacter = interacter;
        }

        public void LogIn()
        {
            connecter.Connect();
        }

        public void CreateChat()
        {
            interacter.WriteLine("Write users id separated by space");
            var users = interacter.ReadLine().Split();
            dialog = connecter.StartDialog(users);
        }

        public void SendMessage()
        {
            if (messageStream.CanReadLine())
                dialog.sendMessage(messageStream.ReadLine());
        }

        public void GetMessages()
        {
            if (!dialog.getMessages(out var messages)) return;
            foreach (var message in messages)
            {
                Console.WriteLine(message);
                messageStream.WriteLine("", message);
            }
        }
    }
}