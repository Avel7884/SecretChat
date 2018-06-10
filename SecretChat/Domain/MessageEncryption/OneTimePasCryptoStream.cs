using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var bytes = Encoding.UTF8.GetBytes(toRead);
            var buffer = new byte[bytes.Length];
            keyReader.ReadKey(buffer, 0, bytes.Length);
            var sb = new StringBuilder();
            return string.Join("", bytes
                            .Select((t, i) => (t ^ buffer[i]).ToString("D3")));
        }

        public void WriteLine(string ps, string toWrite)
        {
            if (!toWrite.All(c => '0' <= c && c <= '9'))
            {
                UnderlayingWriter.WriteLine("Sorry, I can't decode this.\n" + ps + toWrite);
                return;
            }
            var bytes = new Byte[toWrite.Length / 3];
            var buffer = new byte[bytes.Length];
            keyReader.ReadKey(buffer, 0, buffer.Length);
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = (byte) (int.Parse(toWrite.Substring(i * 3, 3)) ^ buffer[i]);
            }
            UnderlayingWriter.WriteLine(ps + Encoding.UTF8.GetString(bytes)); 
        }
    }
}

