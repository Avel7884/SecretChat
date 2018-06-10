using System;
using System.Collections.Generic;

namespace SecretChat
{
    public interface IDialog: IDisposable 
    {
        bool getMessages(out List<Message> messages);
        bool sendMessage(string messages);
    }
}
