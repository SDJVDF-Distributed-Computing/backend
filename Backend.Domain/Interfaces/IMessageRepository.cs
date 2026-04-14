namespace Backend.Domain.Interfaces;

public interface IMessageRepository
{
    Task SaveAsync(ReceivedMessage message, CancellationToken ct = default);
    Task<IReadOnlyList<ReceivedMessage>> GetAllAsync(CancellationToken ct = default);
}