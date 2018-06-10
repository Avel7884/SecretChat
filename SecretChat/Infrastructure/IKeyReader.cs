using System;

namespace SecretChat
{
    public interface IKeyReader : IDisposable
    {
        void ReadKey(byte[] buffer, int offset, int count);
    }
}