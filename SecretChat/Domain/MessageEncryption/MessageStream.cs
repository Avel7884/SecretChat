using System.IO;

namespace SecretChat
{
    public abstract class MessageStream
    {
        private TextReader UnderlayingReader;
        private TextWriter UnderlayingWriter;
        
        public abstract string ReadMessage();
        public abstract void WriteMessage(string ps, string toWrite);
    }
}
