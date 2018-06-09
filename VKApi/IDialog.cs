using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi
{
    public interface IDialog: IDisposable
    {
        bool hasCallback { get; }
        bool getMessages(out List<string> messages);
        bool sendMessage(string messages);
        event EventHandler Callback;
    }
}
