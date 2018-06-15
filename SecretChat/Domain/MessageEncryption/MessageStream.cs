using System.IO;

namespace SecretChat.Domain.MessageEncryption
{
    public interface IMessageStream<T> where T : IMessage
    {
        T ReadMessage();
        void WriteMessage(T message);
    }
}
