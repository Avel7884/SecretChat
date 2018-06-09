using System;

namespace SecretChat
{
    public class ConsoleInterractor : IInteracter
    {
        public void WriteLine(string toWrite)
        {
            Console.WriteLine(toWrite);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}