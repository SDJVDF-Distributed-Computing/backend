namespace Backend.Domain.ValueObjects;

public class Text : ValueObject
{
    public string Value { get; }

    private Text(string value) => Value = value;

    public static Text Create(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var violations = new List<string>();

        if (text.Length > 255)
            violations.Add("maximum 255 characters");

        if (violations.Count > 0)
            throw new InvalidMessageException(violations);

        return new Text(text);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}