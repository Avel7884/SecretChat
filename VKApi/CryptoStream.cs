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
        
        public override void Flush()
        {
            throw new System.NotImplementedException();
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

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
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

        public override bool CanRead { get; }
        public override bool CanSeek { get; }
        public override bool CanWrite { get; }
        public override long Length { get; }
        public override long Position { get; set; }
    }
}