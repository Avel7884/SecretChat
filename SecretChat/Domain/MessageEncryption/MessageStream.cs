using System.IO;

namespace SecretChat.Domain.MessageEncryption
{
    public abstract class MessageStream<IMessage>
    {
        private TextReader UnderlayingReader;
        private TextWriter UnderlayingWriter;
        
        public abstract IMessage ReadMessage();
        public abstract void WriteMessage(IMessage message);
    }
}
