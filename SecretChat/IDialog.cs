using System;
using System.Collections.Generic;

namespace SecretChat
{
    public interface IDialog: IDisposable
    {
        bool hasCallback { get; }
        bool getMessages(out List<string> messages);
        bool sendMessage(string messages);
        event EventHandler Callback;
    }
}
