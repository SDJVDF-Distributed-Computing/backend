namespace Backend.Application.Auth.Commands.Authenticate;

public class AuthenticateCommandHandler(
Session session,
ISessionService sessionService)
{
    public async Task<Result> Handle(
        AuthenticateCommand command,
        CancellationToken ct = default)
    {
        Credentials credentials;
        try
        {
            credentials = Credentials.Create(command.Username, command.Password);
        }
        catch (Exception)
        {
            return Result.Failure(AuthErrors.InvalidCredentials);
        }
        
        await sessionService.AuthenticateAsync(credentials, ct);
        
        session.MarkAsAuthenticated(credentials.Username);
        
        return Result.Success();
    }
}