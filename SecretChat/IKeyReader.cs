using System;

namespace SecretChat
{
    public interface IKeyReader : IDisposable
    {
        int Read(char[] buffer, int offset, int count);
    }
}