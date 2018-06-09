namespace VKApi
{
    public interface IMessanger
    {
        void LogIn();
        void CreateChat();

        void SendMessage();
        void GetMessages();
    }
}