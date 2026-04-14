namespace Backend.Console;

internal sealed class SecureMessageProtocolCLI(IEnumerable<ICLICommand> commands)
{
    private readonly IReadOnlyDictionary<string, ICLICommand> _commands =
        commands.ToDictionary(c => c.Name);

    public async Task RunAsync()
    {
        PrintBanner();

        while (true)
        {
            System.Console.Write("> ");
            var input = System.Console.ReadLine()?.Trim().ToLowerInvariant();

            if (string.IsNullOrEmpty(input)) continue;

            try
            {
                if (!_commands.TryGetValue(input, out var command))
                {
                    System.Console.WriteLine($"Unknown command. Available: {string.Join(" | ", _commands.Keys)}");
                    continue;
                }

                var quit = await command.ExecuteAsync();
                if (quit) return;
            }
            catch (DomainException ex) { System.Console.WriteLine($"[Domain error] {ex.Message}"); }
            catch (Exception ex)       { System.Console.WriteLine($"[Error] {ex.Message}"); }

            System.Console.WriteLine();
        }
    }

    private void PrintBanner()
    {
        System.Console.WriteLine("SMP Client");
        System.Console.WriteLine($"Commands: {string.Join(" | ", _commands.Keys)}");
        System.Console.WriteLine();
    }
}