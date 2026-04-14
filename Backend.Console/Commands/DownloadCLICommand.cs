namespace Backend.Console.Commands;

internal sealed class DownloadCLICommand(DownloadCommandHandler handler) : ICLICommand
{
    public string Name => "download";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var result = await handler.Handle(new DownloadCommand(), ct);

        if (result.IsSuccess)
            CLIOutput.PrintMessages(result.Value!, "Downloaded");
        else
            CLIOutput.PrintError(result.Error!);

        return false;
    }
}
