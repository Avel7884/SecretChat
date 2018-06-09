using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace VKApi
{
    public class OneTimePasCryptoStream : MessageStream
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

        public override bool TryReadLine(out string result)
        {
            var toRead = UnderlayingReader.ReadLine();
            if (toRead == null)
            {
                result = null;
                return false;
            }
            var buffer = new char[toRead.Length];
            keyReader.Read(buffer, 0, toRead.Length);
            
            for (int i = 0; i < toRead.Length; ++i)
                buffer[i] = (char) (toRead[i] ^ buffer[i]);
            result = new StringBuilder()
                .Append(buffer)
                .ToString();
            return result.Length != 0;
        }

        public override void WriteLine(string ps, string toWrite)
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
