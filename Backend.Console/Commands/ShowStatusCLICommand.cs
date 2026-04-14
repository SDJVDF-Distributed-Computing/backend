namespace Backend.Console.Commands;

internal sealed class ShowStatusCLICommand(GetSessionStatusQueryHandler handler) : ICLICommand
{
    public string Name => "status";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var result = await handler.Handle(new GetSessionStatusQuery(), ct);
        var s = result.Value!;
        System.Console.WriteLine($"  Connected:     {s.IsConnected}");
        System.Console.WriteLine($"  Authenticated: {s.IsAuthenticated}");
        System.Console.WriteLine($"  Closed:        {s.IsClosed}");
        return false;
    }
}
