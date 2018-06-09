using System;
using System.Net;
using System.Threading;

namespace SecretChat
{
    public class WebServer
    {
        private readonly HttpListener listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> responder;

        public WebServer(Func<HttpListenerRequest, string> method,string pref)
        {
            listener.Prefixes.Add(pref);
            responder = method ?? throw new ArgumentException("method");
            listener.Start();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Started");
                while (listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem((c) =>
                    {
                        var ctx = c as HttpListenerContext;
                        try
                        {
                            string rstr = responder(ctx.Request);
                            //ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                        }
                        catch { } 
                        //finally
                        //{
                        //    ctx.Response.OutputStream.Close();
                        //}
                    }, listener.GetContext());
                }
            });
        }

        public void Stop()
        {
            listener.Stop();
            listener.Close();
        }
    }
}