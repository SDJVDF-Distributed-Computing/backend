namespace Backend.Console.Output;

internal static class CLIOutput
{
    public static string Prompt(string label, string? defaultValue = null)
    {
        var hint = defaultValue is not null ? $" [{defaultValue}]" : string.Empty;
        System.Console.Write($"  {label}{hint}: ");
        var value = System.Console.ReadLine()?.Trim();
        return string.IsNullOrEmpty(value) && defaultValue is not null ? defaultValue : value ?? string.Empty;
    }

    public static void PrintResult(Result result, string successMessage)
    {
        if (result.IsSuccess)
            System.Console.WriteLine(successMessage);
        else
            PrintError(result.Error!);
    }

    public static void PrintMessages(IReadOnlyList<MessageDTO> messages, string label)
    {
        if (messages.Count == 0)
        {
            System.Console.WriteLine($"No {label.ToLower()} messages.");
            return;
        }

        System.Console.WriteLine($"{label} {messages.Count} message(s):");
        foreach (var msg in messages)
            System.Console.WriteLine($"  [{msg.ReceivedAt:HH:mm:ss}] {msg.Content}");
    }

    public static void PrintError(Error error) =>
        System.Console.WriteLine($"[{error.Code}] {error.Message}");
}
