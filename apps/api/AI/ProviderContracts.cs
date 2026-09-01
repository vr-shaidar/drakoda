namespace Drakoda.AI;

public interface IAIProviderAdapter
{
    string ProviderId { get; }
    bool Supports(GenerationMode mode, string externalModelId);
    Task<ProviderSubmission> SubmitAsync(ProviderContext context, CancellationToken cancellationToken);
    Task<ProviderResult> GetResultAsync(ProviderContext context, CancellationToken cancellationToken);
    Task CancelAsync(ProviderContext context, CancellationToken cancellationToken);
}

public sealed record ProviderContext(Guid GenerationId, string ExternalModelId, GenerationMode Mode, string Prompt, IReadOnlyDictionary<string, object?> Settings, IReadOnlyList<string> SourceUris, string? ExternalJobId = null);

public interface IProviderRouter
{
    IAIProviderAdapter Resolve(string providerId);
    IAIProviderAdapter ResolveFor(string providerId, string externalModelId, GenerationMode mode);
}

public sealed class ProviderRouter(IEnumerable<IAIProviderAdapter> adapters) : IProviderRouter
{
    private readonly IReadOnlyList<IAIProviderAdapter> _adapters = adapters.ToArray();
    public IAIProviderAdapter Resolve(string providerId) => _adapters.FirstOrDefault(x => string.Equals(x.ProviderId, providerId, StringComparison.OrdinalIgnoreCase)) ?? throw new InvalidOperationException($"Provider '{providerId}' is not configured.");
    public IAIProviderAdapter ResolveFor(string providerId, string externalModelId, GenerationMode mode)
    {
        var adapter = Resolve(providerId);
        if (!adapter.Supports(mode, externalModelId)) throw new InvalidOperationException($"Provider '{providerId}' does not support {mode} for model '{externalModelId}'.");
        return adapter;
    }
}

public sealed class UnconfiguredProviderAdapter : IAIProviderAdapter
{
    public string ProviderId => "unconfigured";
    public bool Supports(GenerationMode mode, string externalModelId) => false;
    public Task<ProviderSubmission> SubmitAsync(ProviderContext context, CancellationToken cancellationToken) => throw new InvalidOperationException("No AI provider is configured.");
    public Task<ProviderResult> GetResultAsync(ProviderContext context, CancellationToken cancellationToken) => throw new InvalidOperationException("No AI provider is configured.");
    public Task CancelAsync(ProviderContext context, CancellationToken cancellationToken) => throw new InvalidOperationException("No AI provider is configured.");
}
