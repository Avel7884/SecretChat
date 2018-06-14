using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SecretChat
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
