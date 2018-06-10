using System.Collections.Generic;

namespace SecretChat
{
    public interface IApiRequests
    {
        string SendRequest(string method, Dictionary<string, string> parametrs);
    }
}