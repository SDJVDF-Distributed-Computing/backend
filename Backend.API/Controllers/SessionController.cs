namespace Backend.API.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController(
    GetSessionStatusQueryHandler getSessionStatusQueryHandler,
    DisconnectCommandHandler disconnectCommandHandler,
    AuthenticateCommandHandler authenticateCommandHandler,
    ConnectCommandHandler connectCommandHandler
    ) : ControllerBase
{
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus(CancellationToken ct)
    {
        var result = await getSessionStatusQueryHandler.Handle(new GetSessionStatusQuery(), ct);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => BadRequest(error.ToResponse()));
    }

    [HttpPost("connect")]
    public async Task<IActionResult> Connect(
        [FromBody] ConnectRequest request,
        CancellationToken ct)
    {
        var result = await connectCommandHandler.Handle(new ConnectCommand(request.Host, request.Port), ct);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => BadRequest(error.ToResponse()));
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(
        [FromBody] AuthenticateRequest request,
        CancellationToken ct)
    {
        var result = await authenticateCommandHandler.Handle(new AuthenticateCommand(request.Username, request.Password), ct);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(),
            onFailure: error => error.Code switch
            {
                "auth.credentials.invalid" => Unauthorized(error.ToResponse()),
                _                          => BadRequest(error.ToResponse())
            });
    }

    [HttpDelete("")]
    public async Task<IActionResult> Disconnect(CancellationToken ct)
    {
        var result = await disconnectCommandHandler.Handle(new DisconnectCommand(), ct);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(),
            onFailure: error => BadRequest(error.ToResponse()));
    }
}
