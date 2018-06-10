using System;

namespace SecretChat
{
    public interface IVkApiRequests : IApiRequests
    {
        void SetToken(Func<string> token);
        void SetVersion(string ver);
    }
}