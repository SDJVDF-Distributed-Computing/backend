namespace Backend.Domain.ValueObjects;

public sealed class MessageContent : ValueObject
{
    public string Value { get; }

    private MessageContent(string value) => Value = value;

    public static MessageContent Create(string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));

        var violations = new List<string>();

        if (content.Length > 500)
            violations.Add("maximum 500 characters allowed");

        if (content.Contains('\n') || content.Contains('\r'))
            violations.Add("newlines are not permitted");

        if (violations.Count > 0)
            throw new InvalidMessageException(violations);

        return new MessageContent(content);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
