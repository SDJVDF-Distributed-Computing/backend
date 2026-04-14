namespace Backend.Domain.Aggregates;

public sealed class Message: BaseEntity
{
    public Guid UserId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public DateTime? Timestamp { get; private set; }

    public static Message Create(Guid userId, string text)
    {
        var message = new Message
        {
            UserId = userId,
            Text = text,
        };

        return message;
    }
}