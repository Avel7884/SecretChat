namespace SecretChat
{
    public class Message
    {
        public string Content;
        public string Sender;

        public Message(string content, string sender)
        {
            Content = content;
            Sender = sender;
        }
    }
}