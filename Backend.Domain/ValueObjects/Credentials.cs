namespace Backend.Domain.ValueObjects;

public sealed class Credentials : ValueObject
{
    public string Username { get; }
    public string Password { get; }

    private Credentials(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public static Credentials Create(string username, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        return new Credentials(username, password);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Username;
        yield return Password;
    }
}
