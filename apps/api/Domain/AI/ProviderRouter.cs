namespace Drakoda.Api.Domain.AI;

public interface IProviderRouter
{
    IAIProviderAdapter Resolve(string providerKey);
}

public sealed class ProviderRouter(IEnumerable<IAIProviderAdapter> adapters) : IProviderRouter
{
    private readonly IReadOnlyDictionary<string, IAIProviderAdapter> _adapters = adapters
        .ToDictionary(x => x.ProviderKey, StringComparer.OrdinalIgnoreCase);

    public IAIProviderAdapter Resolve(string providerKey) =>
        _adapters.TryGetValue(providerKey, out var adapter)
            ? adapter
            : throw new InvalidOperationException($"AI provider '{providerKey}' is not registered.");
}

public sealed class UnavailableProviderAdapter : IAIProviderAdapter
{
    public string ProviderKey => "unconfigured";
    public Task<ProviderSubmission> SubmitAsync(ProviderContext context, CancellationToken cancellationToken) =>
        Task.FromResult(new ProviderSubmission("", false, null, new ProviderError("PROVIDER_NOT_CONFIGURED", "No provider adapter is configured for this model.", false)));
    public Task<ProviderOperation> GetOperationAsync(ProviderContext context, string externalJobId, CancellationToken cancellationToken) =>
        Task.FromResult(new ProviderOperation(false, true, null, new ProviderError("PROVIDER_NOT_CONFIGURED", "No provider adapter is configured for this model.", false)));
    public Task CancelAsync(ProviderContext context, string externalJobId, CancellationToken cancellationToken) => Task.CompletedTask;
}
