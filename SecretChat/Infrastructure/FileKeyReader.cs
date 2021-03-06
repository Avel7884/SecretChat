﻿using System.IO;

namespace SecretChat.Infrastructure
{
    public class FileKeyReader : IKeyReader
    {
        private const string PathToKey = "../../Infrastructure/Addition files/key.crypt";
        private BinaryReader keyReader;
        
        public FileKeyReader()
        {
            keyReader = new BinaryReader(File.Open(PathToKey, FileMode.Open));
        }

        public void Dispose()
        {
            keyReader.Close();
        }

        public void ReadKey(byte[] buffer, int offset, int count)
        {
            var temporaryBuffer = new byte[count];
            ReadExactlyCount(temporaryBuffer, count);
    
            for (var i = offset; i < count + offset; ++i)
                buffer[i] = temporaryBuffer[i - offset];
        }

        private void ReadExactlyCount(byte[] buffer, int count)
        {
            var readed = keyReader.Read(buffer, 0, count);
            while (readed != count)
            {
                keyReader.Close();
                keyReader = new BinaryReader(File.Open(PathToKey, FileMode.Open));
                readed += keyReader.Read(buffer, readed, count - readed);
            }
        }
    }
    
    
}