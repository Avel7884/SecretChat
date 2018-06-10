using System;

namespace SecretChat
{
    public class Messanger : IMessanger
    {
        private IConnecter<IDialog> connecter;
        private MessageStream messageStream;
        private IInteracter interacter;
        private IDialog dialog;

        public Messanger(IConnecter<IDialog> connecter, MessageStream messageStream, IInteracter interacter)
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
