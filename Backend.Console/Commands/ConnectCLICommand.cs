namespace Backend.Console.Commands;

internal sealed class ConnectCLICommand(ConnectCommandHandler handler) : ICLICommand
{
    public string Name => "connect";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var host    = CLIOutput.Prompt("Host", "localhost");
        var portStr = CLIOutput.Prompt("Port", "8443");

        if (!int.TryParse(portStr, out var port))
        {
            System.Console.WriteLine("Invalid port number.");
            return false;
        }

        var result = await handler.Handle(new ConnectCommand(host, port), ct);
        CLIOutput.PrintResult(result, "Connected successfully.");
        return false;
    }
}
