using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi
{
    public interface IConnecter<out T> where T : IDialog
    {
        void Connect();
        T StartDialog(IEnumerable ids);
        Dictionary<string,string> GetFriends();
    }
}
