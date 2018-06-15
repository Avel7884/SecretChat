using System.Collections;
using System.Collections.Generic;

namespace SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger
{
    public interface IConnecter<out T> where T : IDialog
    {
        void Connect();
        T StartDialog();
    }
}
