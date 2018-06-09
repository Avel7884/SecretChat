using System.IO;

namespace VKApi
{
    public abstract class MessageStream
    {
        private TextReader UnderlayingReader;
        private TextWriter UnderlayingWriter;
               
        public abstract bool TryReadLine(out string toRead);
        public abstract void WriteLine(string ps, string toWrite);
    }
}
