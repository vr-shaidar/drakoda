namespace Drakoda.Api.Domain.AI;

public enum MediaType { Image, Video, Audio }
public enum GenerationStatus { Requested, Validating, CostEstimated, CreditsReserved, Queued, Submitted, Processing, Downloading, Storing, Completed, ValidationFailed, ModerationFailed, ProviderFailed, Timeout, Cancelled, BillingFailed }

public sealed record GenerationRequest(
    Guid ModelId,
    MediaType MediaType,
    string Prompt,
    IReadOnlyDictionary<string, object?> Settings,
    IReadOnlyList<Guid> SourceAssetIds,
    string? IdempotencyKey);

public sealed record GenerationResult(
    string ExternalJobId,
    IReadOnlyList<GeneratedOutput> Outputs,
    ProviderUsage Usage,
    string? ProviderRequestId);

public sealed record GeneratedOutput(string MediaType, string StorageKey, string ContentType, long? SizeBytes, int? Width, int? Height, double? DurationSeconds);
public sealed record ProviderUsage(decimal? InputTokens, decimal? OutputTokens, decimal? OutputUnits, double? DurationSeconds, decimal? Quantity);

public sealed record ProviderError(string Code, string Message, bool Retryable, int? RetryAfterSeconds = null);

public interface IAIProviderAdapter
{
    string ProviderKey { get; }
    Task<ProviderSubmission> SubmitAsync(ProviderContext context, CancellationToken cancellationToken);
    Task<ProviderOperation> GetOperationAsync(ProviderContext context, string externalJobId, CancellationToken cancellationToken);
    Task CancelAsync(ProviderContext context, string externalJobId, CancellationToken cancellationToken);
}

public sealed record ProviderContext(Guid ProviderId, Guid ModelId, string ModelIdentifier, MediaType MediaType, string Prompt, IReadOnlyDictionary<string, object?> Settings, IReadOnlyList<ProviderInput> Inputs);
public sealed record ProviderInput(string StorageKey, string ContentType);
public sealed record ProviderSubmission(string ExternalJobId, bool Completed, GenerationResult? Result, ProviderError? Error);
public sealed record ProviderOperation(bool Completed, bool Failed, GenerationResult? Result, ProviderError? Error);
