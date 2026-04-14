namespace Backend.Console.Commands;

internal sealed class UploadCLICommand(UploadCommandHandler handler) : ICLICommand
{
    public string Name => "upload";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var text   = CLIOutput.Prompt("Message");
        var result = await handler.Handle(new UploadCommand(text), ct);
        CLIOutput.PrintResult(result, "Message uploaded.");
        return false;
    }
}
