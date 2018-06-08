using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi
{
    interface IConnection<T> where T : IDialog
    {
        void Connect();
        T StartDialog(string[] ids);
        Dictionary<string,string> GetFriends();
    }
}
