using System.Collections.Generic;

namespace SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger
{
    public interface IApiRequests
    {
        string SendRequest(string method, Dictionary<string, string> parametrs);
    }
}