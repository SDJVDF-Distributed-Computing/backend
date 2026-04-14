namespace Backend.Console.Commands;

internal sealed class QuitCLICommand(DisconnectCommandHandler handler) : ICLICommand
{
    public string Name => "quit";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var result = await handler.Handle(new DisconnectCommand(), ct);
        CLIOutput.PrintResult(result, "Disconnected. Goodbye.");
        return true;
    }
}
