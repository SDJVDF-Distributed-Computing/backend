namespace Backend.Console.Commands;

internal interface ICLICommand
{
    string Name { get; }

    Task<bool> ExecuteAsync(CancellationToken ct = default);
}
