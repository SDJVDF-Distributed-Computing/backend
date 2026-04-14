namespace Backend.Application.Auth.Queries.GetSessionStatus;

public sealed class GetSessionStatusQueryHandler(Session session)
{
    public Task<Result<SessionStatusDTO>> Handle(
        GetSessionStatusQuery query,
        CancellationToken ct = default)
    {
        var status = new SessionStatusDTO(
            session.IsConnected,
            session.IsAuthenticated,
            session.IsClosed);

        return Task.FromResult(Result<SessionStatusDTO>.Success(status));
    }
}
