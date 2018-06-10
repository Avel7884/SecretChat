using System;
using System.IO;
using Ninject;
using Ninject.Syntax;

namespace SecretChat
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new StandardKernel();
            BindContainer(container);
            
            var messanger = container.Get<IMessanger>(); //new Messenger(new VkConnecter("6495077", new ConsoleInterractor()), new OneTimePasCryptoStream(Console.In, Console.Out, new KeyReader()), new ConsoleInterractor());

            while (true)
            {
                messanger.GetMessages();
                messanger.SendMessage();
            }
        }

        private static void BindContainer(IBindingRoot container)
        {
            container.Bind<IVkUsersManager>().To<VKUsersManager>();
            container.Bind<IVkApiRequests>().To<VkApiRequests>().InSingletonScope();
            container.Bind<IInteracter>().To<ConsoleInterractor>().InSingletonScope();
            container.Bind<IMessageStream>().To<OneTimePasCryptoStream>();
            container.Bind<IKeyReader>().To<FileKeyReader>();
            container.Bind<TextReader>().ToConstant(Console.In);
            container.Bind<TextWriter>().ToConstant(Console.Out);
            container.Bind<IDialog>().To<VkDialog>();
            container.Bind<IConnecter<IDialog>>().To<VkConnecter>();
            container.Bind<string>().ToConstant("6495077");
            container.Bind<IMessanger>().To<Messanger>()
                .OnActivation(m =>
            {
                m.LogIn();
                m.CreateChat();
            });
        }
    }
}
