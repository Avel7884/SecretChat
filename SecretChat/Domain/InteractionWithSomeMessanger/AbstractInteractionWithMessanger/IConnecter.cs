using System.Collections;
using System.Collections.Generic;

namespace SecretChat
{
    public interface IConnecter<out T> where T : IDialog
    {
        void Connect();
        T StartDialog(IEnumerable ids);
        Dictionary<string,string> GetFriends();
    }
}
