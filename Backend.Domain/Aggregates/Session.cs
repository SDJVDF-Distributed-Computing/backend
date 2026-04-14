namespace Backend.Domain.Aggregates;

public sealed class Session : BaseEntity
{
    public bool IsConnected { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public bool IsClosed { get; private set; }
    public ServerAddress? Address { get; private set; }
    public string? AuthenticatedUsername { get; private set; }

    private Session() { }

    public static Session Create() => new();

    public void MarkAsConnected(ServerAddress address)
    {
        if (IsConnected) throw new InvalidOperationException("Session is already connected.");

        Address = address;
        IsConnected = true;
        RaiseDomainEvent(new SessionConnectedEvent(address));
    }

    public void MarkAsAuthenticated(string username)
    {
        if (!IsConnected) throw new InvalidOperationException("Session must be connected before authenticating.");
        if (IsClosed) throw new InvalidOperationException("Session is closed.");
        if (IsAuthenticated) throw new InvalidOperationException("Session is already authenticated.");

        AuthenticatedUsername = username;
        IsAuthenticated = true;
        RaiseDomainEvent(new SessionAuthenticatedEvent(username));
    }

    public void RecordUpload(string content)
    {
        if (IsClosed) throw new InvalidOperationException("Session is closed.");
        if (!IsAuthenticated) throw new SessionNotAuthenticatedException();

        RaiseDomainEvent(new MessageUploadedEvent(content));
    }

    public void RecordDownload(int messageCount)
    {
        if (IsClosed) throw new InvalidOperationException("Session is closed.");
        if (!IsAuthenticated) throw new SessionNotAuthenticatedException();

        RaiseDomainEvent(new MessagesDownloadedEvent(messageCount));
    }

    public void Close()
    {
        if (!IsConnected) throw new InvalidOperationException("Cannot close a session that is not connected.");
        if (IsClosed) throw new InvalidOperationException("Session is already closed.");

        IsClosed = true;
        IsConnected = false;
        IsAuthenticated = false;
        RaiseDomainEvent(new SessionClosedEvent());
    }

    public void Reset()
    {
        IsConnected = false;
        IsAuthenticated = false;
        IsClosed = false;
        Address = null;
        AuthenticatedUsername = null;
    }
}
