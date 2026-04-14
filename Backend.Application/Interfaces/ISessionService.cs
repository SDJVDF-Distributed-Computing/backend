namespace Backend.Application.Interfaces;

public interface ISessionService
{
    Task ConnectAsync(ServerAddress address, CancellationToken ct = default);
    Task AuthenticateAsync(Credentials credentials, CancellationToken ct = default);
    Task UploadAsync(MessageContent content, CancellationToken ct = default);
    Task<IReadOnlyList<string>> DownloadAsync(CancellationToken ct = default);
    Task DisconnectAsync(CancellationToken ct = default);
}
