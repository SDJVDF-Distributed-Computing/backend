namespace Backend.Infrastructure.Smp.Repositories;

public sealed class InMemoryMessageRepository : IMessageRepository
{
    private readonly List<ReceivedMessage> _messages = [];
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task SaveAsync(ReceivedMessage message, CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            _messages.Add(message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IReadOnlyList<ReceivedMessage>> GetAllAsync(CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            return _messages.ToList().AsReadOnly();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
