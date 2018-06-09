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
//            var connection = 
//            messanger.LogIn();
//            messanger.CreateChat();
            while (true)
            {
                messanger.GetMessages();
                messanger.SendMessage();
            }
//            connection.Connect();
//            var dialog = connection.StartDialog(new string[0]);//new[] { "134650397" }
//            while (execConsoleCommand(dialog));
//            dialog.Dispose();
        }

        private static void BindContainer(IBindingRoot container)
        {
            container.Bind<IInteracter>().To<ConsoleInterractor>().InSingletonScope();
            container.Bind<IMessageStream>().To<OneTimePasCryptoStream>();
            container.Bind<IKeyReader>().To<KeyReader>();
            container.Bind<TextReader>().ToConstant(Console.In);
            container.Bind<TextWriter>().ToConstant(Console.Out);
            container.Bind<IDialog>().To<VKDialog>();
            container.Bind<IConnecter<IDialog>>().To<VKConnecter>();
            container.Bind<string>().ToConstant("6495077");
            container.Bind<IMessanger>().To<Messanger>()
                .OnActivation(m =>
            {
                m.LogIn();
                m.CreateChat();
            });
        }

//        private static bool execConsoleCommand(IDialog dial)
//        {
//            var res = Console.ReadLine().Split();
//            if (res.Length < 0)
//                return false;
//            switch (res[0])
//            {
//                case "w":
//                    return dial.sendMessage(res[1]);
//                case "r":
//                    return printMSGS();
//                case "q":
//                    return false;
//                default:
//                    throw new Exception();
//            }
////            bool printMSGS()
////            {
////                var tmp = dial.getMessages(out string[] asd);
////                foreach (var msg in asd)
////                    Console.WriteLine(msg);
////                return tmp;
////            }
//        }

        //static void InitToken()
        //{
        //private const string home = "http://94.31.140.217:8000/";
        //System.Diagnostics.Process.Start(autrize_request);
        //var code = Console.ReadLine();
        //var res = String.Format("https://oauth.vk.com/access_token?client_id=6495077&client_secret={0}&code={1}", secret, code).GetAsync().Result;
        //Console.WriteLine(res);
        //var token = res.Content.ReadAsStringAsync().Result.Split(new []{'"'},5)[3];
        //res = String.Format("https://api.vk.com/method/messages.send?chat_id=25&message=Test%20with%20Flurl&access_token={0}&v=5.78",token).GetAsync().Result;
        //for(int i=0;i<1000;i++)
        //    res=String.Format("https://api.vk.com/method/messages.send?chat_id=25&message=Test%20with%20Flurl&access_token={0}&v=5.78", token).GetAsync().Result;
        //Console.WriteLine(res);
        //Console.ReadKey();
        //WebServer ws = new WebServer(request => {
        //    code = request.QueryString["code"];
        //    return "";
        //}, "http://127.0.0.1:8000/");
        //ws.Run();
        //while (code == null)
        //    Thread.Sleep(10);
        //ws.Stop();
        //Console.WriteLine(code);

        //var tkn = String.Format("https://oauth.vk.com/access_token?client_id=1&client_secret={0}&redirect_uri={1}&code={2}", secret, home, code).GetAsync().Result;
        //Console.WriteLine(tkn.Content.);
        //}

        //static string output = "";

        //public void createListener()
        //{
        //    // Create an instance of the TcpListener class.
        //    TcpListener tcpListener = null;
        //    IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
        //    try
        //    {
        //        // Set the listener on the local IP address
        //        // and specify the port.
        //        tcpListener = new TcpListener(ipAddress, 13);
        //        tcpListener.Start();
        //        tcpListener.
        //        output = "Waiting for a connection...";
        //    }
        //    catch (Exception e)
        //    {
        //        output = "Error: " + e.ToString();
        //    }
        //    while (true)
        //    {
        //        // Always use a Sleep call in a while(true) loop
        //        // to avoid locking up your CPU.
        //        Thread.Sleep(10);
        //        // Create a TCP socket.
        //        // If you ran this server on the desktop, you could use
        //        // Socket socket = tcpListener.AcceptSocket()
        //        // for greater flexibility.
        //        TcpClient tcpClient = tcpListener.AcceptTcpClient();
        //        // Read the data stream from the client.
        //        byte[] bytes = new byte[256];
        //        NetworkStream stream = tcpClient.GetStream();
        //        stream.Read(bytes, 0, bytes.Length);
        //        SocketHelper helper = new SocketHelper();
        //        helper.processMsg(tcpClient, stream, bytes);
        //    }
        //}
    }
}
