namespace Backend.Application.Auth.Commands.Connect;

public class ConnectCommandHandler(
Session session,
ISessionService sessionService)
{
    public async Task<Result> Handle(
        ConnectCommand command,
        CancellationToken ct = default)
    {
        ServerAddress serverAddress;
        try
        {
            serverAddress = ServerAddress.Create(command.Host, command.Port);
        }
        catch (Exception)
        {
            return Result.Failure(AuthErrors.InvalidServerAddress);
        }
        
        if (session.IsConnected || session.IsClosed)
            session.Reset();

        await sessionService.ConnectAsync(serverAddress, ct);
        session.MarkAsConnected(serverAddress);

        return Result.Success();
    }
}