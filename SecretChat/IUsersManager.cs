using System.Collections.Generic;

namespace SecretChat
{
    public interface IUsersManager
    {
        string GetNameById(string id);
        List<string> GetIdsByUser(string name);
    }
}