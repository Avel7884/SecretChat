namespace SecretChat.Application
{
    public interface IMessanger
    {
        void LogIn();
        void CreateChat();

        void SendMessage();
        void GetMessages();
    }
}