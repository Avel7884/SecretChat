namespace SecretChat
{
    public class Message : IMessage
    {
        public string Content { get; }
        public string Head => $"({sender})> ";
        public string Tail => "";
        private readonly string sender;

        public Message(string content, string sender="")
        {
            Content = content;
            this.sender = sender;
        }
        
        public override string ToString()
        {
            return Head + Content + Tail;
        }
    }
}