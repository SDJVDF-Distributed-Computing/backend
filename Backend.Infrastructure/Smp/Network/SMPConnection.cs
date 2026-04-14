namespace Backend.Infrastructure.Smp.Network;

public sealed class SMPConnection : IAsyncDisposable
{
    private readonly ServerAddress _address;
    private TcpClient? _tcpClient;
    private SslStream? _sslStream;
    private StreamReader? _reader;
    private StreamWriter? _writer;

    private readonly string _certPath;

    public SMPConnection(ServerAddress address, string certPath)
    {
        _address = address;
        _certPath = certPath;
    }

    public async Task ConnectAsync(CancellationToken ct = default)
    {
        _tcpClient = new TcpClient();
        await _tcpClient.ConnectAsync(_address.Host, _address.Port, ct);

        _sslStream = new SslStream(
            _tcpClient.GetStream(),
            leaveInnerStreamOpen: false);

        await _sslStream.AuthenticateAsClientAsync(
            new SslClientAuthenticationOptions
            {
                TargetHost = _address.Host,
                RemoteCertificateValidationCallback = ValidateCertificate
            },
            ct);

        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        _writer = new StreamWriter(_sslStream, encoding) { AutoFlush = true, NewLine = "\n" };
        _reader = new StreamReader(_sslStream, encoding);
    }

    public async Task SendLineAsync(string line, CancellationToken ct = default)
    {
        EnsureConnected();
        await _writer!.WriteAsync(line.AsMemory(), ct);
    }

    public async Task<string> ReadLineAsync(CancellationToken ct = default)
    {
        EnsureConnected();
        var line = await _reader!.ReadLineAsync(ct);
        return line ?? throw new IOException("Server closed the connection unexpectedly.");
    }

    private bool ValidateCertificate(
        object sender,
        X509Certificate? certificate,
        X509Chain? chain,
        SslPolicyErrors sslPolicyErrors)
    {
        if (certificate is null) return false;

        if (!File.Exists(_certPath))
        {
            return true;
        }

        try
        {
            var pinned = X509CertificateLoader.LoadCertificateFromFile(_certPath);
            var server = X509CertificateLoader.LoadCertificate(((X509Certificate2)certificate).RawData);
            return string.Equals(pinned.Thumbprint, server.Thumbprint, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private void EnsureConnected()
    {
        if (_sslStream is null || _writer is null || _reader is null)
            throw new InvalidOperationException("Call ConnectAsync before using the connection.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_writer is not null) await _writer.DisposeAsync();
        _reader?.Dispose();
        if (_sslStream is not null) await _sslStream.DisposeAsync();
        _tcpClient?.Dispose();
    }
}
