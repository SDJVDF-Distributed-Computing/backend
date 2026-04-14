namespace Backend.Application.Messages.Queries.GetCachedMessages;

public sealed class GetCachedMessagesQueryHandler(IMessageRepository messageRepository)
{
    public async Task<Result<IReadOnlyList<MessageDTO>>> Handle(GetCachedMessagesQuery query, CancellationToken ct = default)
    {
        var messages = await messageRepository.GetAllAsync(ct);
        var dtos = messages
            .Select(m => new MessageDTO(m.Id, m.Content, m.ReceivedAt))
            .ToList();
        return Result<IReadOnlyList<MessageDTO>>.Success(dtos);
    }
}
