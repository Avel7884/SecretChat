using System;
using System.IO;

namespace VKApi
{
    public class CryptoStream : Stream
    {
        private Stream underlayingStream;
        private KeyReader keyReader;

        public CryptoStream(Stream underlayingStream, KeyReader keyReader)
        {
            this.underlayingStream = underlayingStream;
            this.keyReader = keyReader;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = underlayingStream.Read(buffer, offset, count);
            var partOfKey = new byte[count];
            keyReader.Read(partOfKey, 0, read);
            for (var i = offset; i < read + offset; ++i)
                buffer[i] = (byte) (partOfKey[i - offset] ^ buffer[i]);
            return read;
        }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            var toWrite = new byte[count];
            var partOfKey = new byte[count];
            keyReader.Read(partOfKey, 0, count);
            for (var i = offset; i < count + offset; ++i)
                toWrite[i - offset] = (byte) (partOfKey[i - offset] ^ buffer[i]);
            underlayingStream.Write(toWrite, 0, count);
        }
        
        public override bool CanRead => underlayingStream.CanRead;

        public override bool CanSeek => underlayingStream.CanSeek;

        public override bool CanWrite => underlayingStream.CanWrite;

        public override void Flush()
        {
            underlayingStream.Flush();
        }

        public override long Length => underlayingStream.Length;

        public override long Position
        {
            get => underlayingStream.Position;
            set => underlayingStream.Position = value;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return underlayingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            underlayingStream.SetLength(value);
        }
    }
}