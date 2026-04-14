namespace Backend.Domain.Entities;

public sealed class ReceivedMessage : BaseEntity
{
    public string Content { get; private set; } = string.Empty;
    public DateTime ReceivedAt { get; private set; }

    private ReceivedMessage() { }

    public static ReceivedMessage Reconstitute(string content, DateTime receivedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
        return new ReceivedMessage
        {
            Content = content,
            ReceivedAt = receivedAt
        };
    }
}
