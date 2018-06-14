namespace SecretChat
{
    public interface IInteracter
    {
        void WriteLine(string toWrite);

        bool CanReadLine();
        string ReadLine();
    }
}
