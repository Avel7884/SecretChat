using System;
using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction
{
    public interface IVkApiRequests : IApiRequests
    {
        Func<string> Token { set; }
        string Ver { set; }
    }
}
