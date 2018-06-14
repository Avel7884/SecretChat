using System;
using System.Collections.Generic;

namespace SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger
{
    public interface IDialog: IDisposable 
    {
        bool getMessages(out List<Message> messages);
        bool sendMessage(IMessage messages);
    }
}
