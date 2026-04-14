namespace Backend.Domain.ValueObjects;

public sealed class ServerAddress : ValueObject
{
    public string Host { get; }
    public int Port { get; }

    private ServerAddress(string host, int port)
    {
        Host = host;
        Port = port;
    }

    public static ServerAddress Create(string host, int port)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(host);
        
        if (port is < 1 or > 65535)
        {
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535.");
        }
        
        return new ServerAddress(host, port);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Host;
        yield return Port;
    }

    public override string ToString() => $"{Host}:{Port}";
}
