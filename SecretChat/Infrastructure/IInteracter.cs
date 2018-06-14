namespace SecretChat.Infrastructure
{
    public interface IInteracter
    {
        void WriteLine(string toWrite);

        bool CanReadLine();
        string ReadLine();
    }
}
