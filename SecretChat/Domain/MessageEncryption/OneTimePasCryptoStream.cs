using System;
using System.IO;
using System.Linq;
using System.Text;
using SecretChat.Infrastructure;

namespace SecretChat.Domain.MessageEncryption
{
    public class OneTimePasCryptoStream : IMessageStream<IMessage>
    {
        private TextReader UnderlayingReader { get; }
        private TextWriter UnderlayingWriter { get; }
        private readonly IKeyReader keyReader;

        public OneTimePasCryptoStream(TextReader reader, TextWriter writer, IKeyReader keyReader)
        {
            UnderlayingReader = reader;
            UnderlayingWriter = writer;
            this.keyReader = keyReader;
        }

        public IMessage ReadMessage()
        {
            var toRead = UnderlayingReader.ReadLine();
            var bytes = Encoding.UTF8.GetBytes(toRead);
            var buffer = new byte[bytes.Length];
            keyReader.ReadKey(buffer, 0, bytes.Length);
            return new Message(string.Join("", bytes
                            .Select((t, i) => (t ^ buffer[i]).ToString("D3"))), "");
        }

        public void WriteMessage(IMessage message)
        {
            var content = message.Content;
            if (!content.All(c => '0' <= c && c <= '9'))
            {
                UnderlayingWriter.WriteLine("Sorry, I can't decode this.\n" + message.ToString());
                return;
            }
            var bytes = new Byte[content.Length / 3];
            var buffer = new byte[bytes.Length];
            keyReader.ReadKey(buffer, 0, buffer.Length);
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = (byte) (int.Parse(content.Substring(i * 3, 3)) ^ buffer[i]);
            }
            UnderlayingWriter.WriteLine(message.Head + Encoding.UTF8.GetString(bytes) + message.Tail); 
        }
    }
}

