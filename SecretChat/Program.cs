using System;
using System.IO;
using Ninject;
using Ninject.Extensions.Conventions;
using SecretChat.Application;
using SecretChat.Domain.InteractionWithSomeMessanger.AbstractInteractionWithMessanger;
using SecretChat.Domain.InteractionWithSomeMessanger.InteractionWithVk.CustomVkInteraction;
using SecretChat.Domain.MessageEncryption;
using SecretChat.Infrastructure;

namespace SecretChat
{
    class Program
    {
        private const string applicationClientId = "6495077";
        static void Main(string[] args)
        {
            var container = new StandardKernel();
            BindContainer(container);
            var messanger = container.Get<IMessenger>();
            while (true)
            {
                messanger.GetMessages();
                messanger.SendMessage();
            }
        }

        private static void BindContainer(StandardKernel container)
        {
            container.Bind(c => c.FromThisAssembly().SelectAllClasses().InheritedFrom<IUsersManager>()
                .BindAllInterfaces().Configure(b => b.InSingletonScope()));
            container.Bind(c => c.FromThisAssembly().SelectAllClasses().InheritedFrom<IApiRequests>()
                .BindAllInterfaces().Configure(b => b.InSingletonScope()));
            container.Bind<IInteracter>().To<ConsoleInterracter>().InSingletonScope();
            container.Bind<IMessage>().To<Message>();
            container.Bind<IMessageStream<IMessage>>().To<OneTimePasCryptoStream>();
            container.Bind<IKeyReader>().To<FileKeyReader>();
            container.Bind<TextReader>().ToConstant(Console.In);
            container.Bind<TextWriter>().ToConstant(Console.Out);
            container.Bind<IDialog>().To<VkDialog>();
            container.Bind<IConnecter<IDialog>>().To<VkConnecter>()
                .WithConstructorArgument("applicationClientId", applicationClientId);
            container.Bind<IMessenger>().To<Messenger>()
                .OnActivation(m =>
                {
                    m.LogIn();
                    m.CreateChat();
                });
        }
    }
}
