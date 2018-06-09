using System.Collections.Generic;

namespace VKApi
{
    public interface IFriendsManager
    {
        string GetNameById(string id);
        IEnumerable<string> GetIdsByName(string name);
    }
}