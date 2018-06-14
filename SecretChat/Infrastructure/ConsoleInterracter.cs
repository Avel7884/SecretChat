using System;

namespace SecretChat.Infrastructure
{
    public class ConsoleInterracter : IInteracter
    {
        public void WriteLine(string toWrite)
        {
            Console.WriteLine(toWrite);
        }

        public bool CanReadLine()
        {
            return Console.In.Peek() != -1;
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
