using System;
using System.IO;

namespace VKApi
{
    public interface IKeyReader : IDisposable
    {
        int Read(char[] buffer, int offset, int count);
    }
}