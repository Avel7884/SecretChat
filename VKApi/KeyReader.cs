using System;
using System.Configuration;
using System.IO;

namespace VKApi
{
    public class KeyReader : Stream, IDisposable
    {
        private const string pathToKey = "key.crypt";
        private StreamReader keyReader;
        private bool isDisposed;
        
        public KeyReader()
        {
            isDisposed = false;
            keyReader = new StreamReader(pathToKey);
        }

        public void Dispose()
        {
             Dispose(true);   
        }

        protected virtual void Dispose(bool fromDisposedMethod)
        {
            if (isDisposed) return;
            isDisposed = true;
            keyReader.Close();
        }
        
        public override void Flush()
        {
            throw new System.NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var temporaryBuffer = new char[count];
            readExactlyCount(temporaryBuffer, count);
    
            for (var i = offset; i < count + offset; ++i)
                buffer[i] = (byte) temporaryBuffer[i - offset];
            return count;
        }

        private void readExactlyCount(char[] buffer, int count)
        {
            var readed = keyReader.Read(buffer, 0, count);
            while (readed != count)
            {
                keyReader.Close();
                keyReader = new StreamReader(pathToKey);
                readed += keyReader.Read(buffer, readed, count - readed);
            }
        }

        public override long Seek(long offset,     SeekOrigin origin)
        {
            throw new System.NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanRead { get; }
        public override bool CanSeek { get; }
        public override bool CanWrite { get; }
        public override long Length { get; }
        public override long Position { get; set; }
    }
}