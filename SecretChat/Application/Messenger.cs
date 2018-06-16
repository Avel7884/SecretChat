using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;
using SecretChat.Domain.MessageEncryption;
using SecretChat.Infrastructure;

namespace SecretChat.Application
{
    public class Messenger : IMessenger
    {
        private IConnecter<IDialog> connecter;
        private IMessageStream<IMessage> messageStream;
        private IInteracter interacter;
        private IDialog dialog;
        private IUsersManager usersManager;

        public Messenger(IConnecter<IDialog> connecter, IMessageStream<IMessage> messageStream, 
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
            dialog = connecter.StartDialog();
            interacter.WriteLine("Connection is successful! You can start messaging!");
        }

        public void SendMessage()
        {
            var message = messageStream.ReadMessage();
            if (dialog.SendMessage(message)) ;
        }

        public void GetMessages()
        {
            if (!dialog.GetMessages(out var messages)) return;
            foreach (Message message in messages)
            {
                messageStream.WriteMessage(message);
            }
        }
    }
}
