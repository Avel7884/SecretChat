using System.Collections.Generic;

namespace SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger
{
    public interface IUsersManager
    {
        string GetNameById(string id);
    }
}
