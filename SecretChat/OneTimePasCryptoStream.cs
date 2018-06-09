using System.IO;
using System.Text;

namespace SecretChat
{
    public class OneTimePasCryptoStream : IMessageStream
    {
        private TextReader UnderlayingReader { get; }
        private TextWriter UnderlayingWriter { get; }
        private readonly IKeyReader keyReader;

        public OneTimePasCryptoStream(TextReader Reader, TextWriter Writer, IKeyReader keyReader)
        {
            UnderlayingReader = Reader;
            UnderlayingWriter = Writer;
            this.keyReader = keyReader;
        }

        public bool CanReadLine()
        {
            return UnderlayingReader.Peek() != -1;
        }

        public string ReadLine()
        {
            var toRead = UnderlayingReader.ReadLine();
            var buffer = new char[toRead.Length];
            keyReader.Read(buffer, 0, toRead.Length);
            
            for (int i = 0; i < toRead.Length; ++i)
                buffer[i] = (char) (toRead[i] ^ buffer[i]);
            return new StringBuilder()
                .Append(buffer)
                .ToString();
        }

        public void WriteLine(string ps, string toWrite)
        {
            var buffer = new char[toWrite.Length];
            keyReader.Read(buffer, 0, toWrite.Length);
            
            for (int i = 0; i < toWrite.Length; ++i)
                buffer[i] = (char) (toWrite[i] ^ buffer[i]);
            UnderlayingWriter.WriteLine(ps, 
                new StringBuilder()
                .Append(buffer));
        }
    }
}
