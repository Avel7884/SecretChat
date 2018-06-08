using System.IO;

namespace VKApi
{
    public interface IStringStream
    {
        bool TryReadLine(out string toRead);
        void WriteLine(string toWrite);
    }
}