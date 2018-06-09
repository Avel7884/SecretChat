using System.IO;

namespace SecretChat
{
    public class KeyReader : IKeyReader
    {
        private const string pathToKey = "../../key.crypt";
        private BinaryReader keyReader;
        
        public KeyReader()
        {
            keyReader = new BinaryReader(File.Open(pathToKey, FileMode.Open));
        }

        public void Dispose()
        {
            keyReader.Close();
        }

        public int Read(char[] buffer, int offset, int count)
        {
            var temporaryBuffer = new char[count];
            ReadExactlyCount(temporaryBuffer, count);
    
            for (var i = offset; i < count + offset; ++i)
                buffer[i] = temporaryBuffer[i - offset];
            return count;
        }

        private void ReadExactlyCount(char[] buffer, int count)
        {
            var readed = keyReader.Read(buffer, 0, count);
            while (readed != count)
            {
                keyReader.Close();
                keyReader = new BinaryReader(File.Open(pathToKey, FileMode.Open));
                readed += keyReader.Read(buffer, readed, count - readed);
            }
        }
    }
}