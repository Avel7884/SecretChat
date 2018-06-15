using System.Collections.Generic;
using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction
{
    public interface IVkUsersManager : IUsersManager
    {
        IEnumerable<string> GetIdsByName(string name);
    }
}
