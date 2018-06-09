using System.Collections.Generic;

namespace SecretChat
{
    public interface IFriendsManager
    {
        string GetNameById(string id);
        IEnumerable<string> GetIdsByName(string name);
    }
}