namespace SecretChat
{
    public interface IMessage
    {
        string Content { get; }
        string Head { get; }
        string Tail { get; }

        string ToString();
    }
}