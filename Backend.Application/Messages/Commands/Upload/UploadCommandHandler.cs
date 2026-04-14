namespace Backend.Application.Messages.Commands.Upload;

public class UploadCommandHandler(
Session session,
ISessionService sessionService)
{
    public async Task<Result> Handle(
        UploadCommand command,
        CancellationToken ct = default)
    {
        MessageContent messageContent;
        try
        {
            messageContent = MessageContent.Create(command.Content);
        }
        catch (Exception)
        {
            return Result.Failure(MessageErrors.InvalidMessageContent);
        }
        
        session.RecordUpload(command.Content);
        
        await sessionService.UploadAsync(messageContent, ct);

        return Result.Success();
    }
}