using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace VKApi
{
    public class OneTimePasCryptoStream : IStringStream
    {
        private TextReader underlayingReader;
        private TextWriter underlayingWriter;
        private IKeyReader keyReader;

        public OneTimePasCryptoStream(TextReader Reader, TextWriter Writer, IKeyReader keyReader)
        {
            underlayingReader = Reader;
            underlayingWriter = Writer;
            this.keyReader = keyReader;
        }

        public bool TryReadLine(out string result)
        {
            var toRead = underlayingReader.ReadLine();
            var buffer = new char[toRead.Length];
            keyReader.Read(buffer, 0, toRead.Length);
            
            for (int i = 0; i < toRead.Length; ++i)
                buffer[i] = (char) (toRead[i] ^ buffer[i]);
            result = new StringBuilder()
                .Append(buffer)
                .ToString();
            return result.Length != 0;
        }

        public void WriteLine(string toWrite)
        {
            var buffer = new char[toWrite.Length];
            keyReader.Read(buffer, 0, toWrite.Length);
            
            for (int i = 0; i < toWrite.Length; ++i)
                buffer[i] = (char) (toWrite[i] ^ buffer[i]);
            underlayingWriter.WriteLine(
                new StringBuilder()
                .Append(buffer)
                .ToString());
        }
    }
}
