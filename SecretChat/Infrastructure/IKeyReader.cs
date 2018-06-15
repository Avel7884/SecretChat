using System;

namespace SecretChat.Infrastructure
{
    public interface IKeyReader : IDisposable
    {
        void ReadKey(byte[] buffer, int offset, int count);
    }
}
