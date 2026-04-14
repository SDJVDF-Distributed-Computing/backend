namespace Backend.Console.Commands;

internal sealed class LoginCLICommand(AuthenticateCommandHandler handler) : ICLICommand
{
    public string Name => "login";

    public async Task<bool> ExecuteAsync(CancellationToken ct = default)
    {
        var username = CLIOutput.Prompt("Username");
        var password = CLIOutput.Prompt("Password");

        var result = await handler.Handle(new AuthenticateCommand(username, password), ct);
        CLIOutput.PrintResult(result, "Authenticated successfully.");
        return false;
    }
}
