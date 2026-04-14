namespace Backend.Application.Auth.Commands.Disconnect;

public class DisconnectCommandHandler(
Session session,
ISessionService sessionService)
{
    public async Task<Result> Handle(
        DisconnectCommand command,
        CancellationToken ct = default)
    {
        await sessionService.DisconnectAsync(ct);
        session.Close();
        
        return Result.Success();
    }
}