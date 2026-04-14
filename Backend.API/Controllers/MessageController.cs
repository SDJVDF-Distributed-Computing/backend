namespace Backend.API.Controllers;

[ApiController]
[Route("api/messages")]
[ServiceFilter(typeof(RequireSessionFilter))]
public sealed class MessageController(
    UploadCommandHandler uploadCommandHandler,
    DownloadCommandHandler downloadCommandHandler,
    GetCachedMessagesQueryHandler getCachedMessagesQueryHandler
    ) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetCached(CancellationToken ct)
    {
        var result = await getCachedMessagesQueryHandler.Handle(new GetCachedMessagesQuery(), ct);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => BadRequest(error.ToResponse()));
    }

    [HttpPost("")]
    public async Task<IActionResult> Upload(
        [FromBody] UploadRequest request,
        CancellationToken ct)
    {
        var result = await uploadCommandHandler.Handle(new UploadCommand(request.Content), ct);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => error.Code switch
            {
                "messages.content.invalid" => UnprocessableEntity(error.ToResponse()),
                _                          => BadRequest(error.ToResponse())
            });
    }

    [HttpPost("download")]
    public async Task<IActionResult> Download(CancellationToken ct)
    {
        var result = await downloadCommandHandler.Handle(new DownloadCommand(), ct);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => BadRequest(error.ToResponse()));
    }
}
