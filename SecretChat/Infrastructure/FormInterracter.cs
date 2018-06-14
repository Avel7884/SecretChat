using System;

namespace SecretChat
{
    public class FormInterracter : IInteracter
    {
        private string text;
        public void WriteLine(string toWrite)
        {
            text = toWrite;
        }

        public bool CanReadLine()
        {
            return text != null;
        }

        public string ReadLine()
        {
            var line = text;
            text = null;
            return line;
        }
    }
}
