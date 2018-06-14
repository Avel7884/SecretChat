using System.IO;

namespace SecretChat.Domain.MessageEncryption
{
    public abstract class MessageStream<T> where T : IMessage
    {
        private TextReader UnderlayingReader;
        private TextWriter UnderlayingWriter;
        
        public abstract T ReadMessage();
        public abstract void WriteMessage(T message);
    }
}
