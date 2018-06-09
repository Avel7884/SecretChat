using System.IO;

namespace SecretChat
{
    public interface IMessageStream
    {
        bool CanReadLine();
        
        string ReadLine();
        void WriteLine(string ps, string toWrite);
    }
}
