// ReSharper disable All
namespace SecretChat.Application
{
    public interface IMessenger
    {
        void LogIn();
        void CreateChat();

        void SendMessage();
        void GetMessages();
    }
}
