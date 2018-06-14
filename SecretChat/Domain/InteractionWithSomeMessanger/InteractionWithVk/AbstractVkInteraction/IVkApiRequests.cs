using System;
using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;

namespace SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.AbstractVkInteraction
{
    public interface IVkApiRequests : IApiRequests
    {
        void SetToken(Func<string> token);
        void SetVersion(string ver);
    }
}