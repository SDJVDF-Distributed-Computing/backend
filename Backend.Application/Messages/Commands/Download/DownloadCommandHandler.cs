namespace Backend.Application.Messages.Commands.Download;

public class DownloadCommandHandler(
Session session,
IMessageRepository messageRepository,
ISessionService sessionService)
{
    public async Task<Result<IReadOnlyList<MessageDTO>>> Handle(
        DownloadCommand command,
        CancellationToken ct = default)
    {
            session.RecordDownload(0);
            var lines = await sessionService.DownloadAsync(ct);

            var dtos = new List<MessageDTO>();
            foreach (var line in lines)
            {
                var msg = ReceivedMessage.Reconstitute(line, DateTime.UtcNow);
                await messageRepository.SaveAsync(msg, ct);
                dtos.Add(new MessageDTO(msg.Id, msg.Content, msg.ReceivedAt));
            }

            return Result<IReadOnlyList<MessageDTO>>.Success(dtos);
    }
}