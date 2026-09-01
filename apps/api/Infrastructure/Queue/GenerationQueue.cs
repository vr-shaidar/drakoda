namespace Drakoda.Api.Infrastructure.Queue;

public interface IGenerationQueue
{
    Task EnqueueAsync(Guid generationId, CancellationToken cancellationToken = default);
    Task<Guid> DequeueAsync(CancellationToken cancellationToken);
}

public sealed class RedisGenerationQueue : IGenerationQueue
{
    private const string QueueKey = "drakoda:generation-jobs";
    private readonly StackExchange.Redis.IConnectionMultiplexer _redis;

    public RedisGenerationQueue(StackExchange.Redis.IConnectionMultiplexer redis) => _redis = redis;

    public Task EnqueueAsync(Guid generationId, CancellationToken cancellationToken = default) =>
        _redis.GetDatabase().ListRightPushAsync(QueueKey, generationId.ToString());

    public async Task<Guid> DequeueAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var value = await _redis.GetDatabase().ListLeftPopAsync(QueueKey);
            if (value.HasValue && Guid.TryParse(value.ToString(), out var id)) return id;
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
        throw new OperationCanceledException(cancellationToken);
    }
}
