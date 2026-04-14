namespace Backend.Infrastructure.Smp.Services;

public sealed class SessionService : ISessionService, IAsyncDisposable
{
    private SMPConnection? _connection;
    private readonly string _certPath;

    public SessionService(string certPath)
    {
        _certPath = certPath;
    }

    public async Task ConnectAsync(ServerAddress address, CancellationToken ct = default)
    {
        _connection = new SMPConnection(address, _certPath);
        await _connection.ConnectAsync(ct);
    }

    public async Task AuthenticateAsync(Credentials credentials, CancellationToken ct = default)
    {
        EnsureConnection();

        await _connection!.SendLineAsync(
            SMPProtocolParser.BuildCommand("HELO", credentials.Username), ct);
        var heloResponse = await ReadResponseAsync(ct);
        EnsureCode(heloResponse, SMPResponseCode.Challenge, $"HELO failed: {heloResponse.Message}");

        await _connection.SendLineAsync(
            SMPProtocolParser.BuildCommand("AUTH", $"{credentials.Username} {credentials.Password}"), ct);
        var authResponse = await ReadResponseAsync(ct);
        EnsureCode(authResponse, SMPResponseCode.Ok, $"AUTH failed: {authResponse.Message}");
    }

    public async Task UploadAsync(MessageContent content, CancellationToken ct = default)
    {
        EnsureConnection();

        await _connection!.SendLineAsync(
            SMPProtocolParser.BuildCommand("UPLD", content.Value), ct);
        var response = await ReadResponseAsync(ct);
        EnsureCode(response, SMPResponseCode.Ok, $"UPLD failed: {response.Message}");
    }

    public async Task<IReadOnlyList<string>> DownloadAsync(CancellationToken ct = default)
    {
        EnsureConnection();

        await _connection!.SendLineAsync(SMPProtocolParser.BuildCommand("DNLD"), ct);

        var messages = new List<string>();
        while (true)
        {
            var response = await ReadResponseAsync(ct);
            if (response.Code == SMPResponseCode.EndMessages) break;
            if (response.Code == SMPResponseCode.Message)
                messages.Add(response.Message);
            else
                throw new InvalidOperationException($"Unexpected code during DNLD: {(int)response.Code} {response.Message}");
        }

        return messages.AsReadOnly();
    }

    public async Task DisconnectAsync(CancellationToken ct = default)
    {
        if (_connection is null) return;

        await _connection.SendLineAsync(SMPProtocolParser.BuildCommand("QUIT"), ct);
        var response = await ReadResponseAsync(ct);
        EnsureCode(response, SMPResponseCode.Ok, $"QUIT failed: {response.Message}");

        await _connection.DisposeAsync();
        _connection = null;
    }

    private async Task<SMPRawResponse> ReadResponseAsync(CancellationToken ct)
    {
        var line = await _connection!.ReadLineAsync(ct);
        return SMPProtocolParser.ParseLine(line);
    }

    private void EnsureConnection()
    {
        if (_connection is null)
            throw new InvalidOperationException("Not connected. Call ConnectAsync first.");
    }

    private static void EnsureCode(SMPRawResponse response, SMPResponseCode expected, string errorMessage)
    {
        if (response.Code != expected)
            throw new InvalidOperationException($"{errorMessage} (received {(int)response.Code})");
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
