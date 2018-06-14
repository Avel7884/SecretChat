using System;
using System.Collections.Generic;

namespace SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger
{
    public interface IDialog
    {
        bool GetMessages(out List<IMessage> messages);
        bool SendMessage(IMessage messages);
    }
}
