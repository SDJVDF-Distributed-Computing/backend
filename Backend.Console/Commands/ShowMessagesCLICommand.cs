namespace Backend.Console.Commands;

internal sealed class ShowMessagesCLICommand(GetCachedMessagesQueryHandler handler) : ICLICommand
{
    public string Name => "messages";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var result = await handler.Handle(new GetCachedMessagesQuery(), ct);
        CLIOutput.PrintMessages(result.Value!, "Cached");
        return false;
    }
}
