using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi
{
    interface IConnecter<T> where T : IDialog
    {
        T Connect();
        string GetName(string id);
    }
}
