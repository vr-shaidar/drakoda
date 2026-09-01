namespace Drakoda.AI;

public enum MediaType { Image, Video, Audio }
public enum GenerationStatus { Requested, Validating, Moderation, CostEstimated, CreditsReserved, Queued, Submitted, Processing, Downloading, Storing, Completed, ValidationFailed, ModerationFailed, ProviderFailed, Timeout, Cancelled, BillingFailed }
public enum GenerationMode { TextToImage, ImageToImage, ImageEdit, TextToVideo, ImageToVideo, VideoTransform, TextToSpeech, AudioGeneration }

public sealed record GenerationRequest(
    Guid UserId,
    GenerationMode Mode,
    Guid ModelId,
    string Prompt,
    IReadOnlyDictionary<string, object?> Settings,
    IReadOnlyList<Guid> SourceAssetIds,
    string? IdempotencyKey);

public sealed record CostEstimate(decimal ProviderCost, decimal CustomerPrice, long Credits, string PricingVersion, string Currency);
public sealed record ProviderSubmission(string ProviderJobId, string Status, IReadOnlyDictionary<string, string>? Metadata = null);
public sealed record ProviderResult(string ProviderJobId, IReadOnlyList<ProviderOutput> Outputs, decimal? ActualProviderCost = null, long? ActualCredits = null);
public sealed record ProviderOutput(string MediaType, string StorageUri, string? MimeType = null, long? SizeBytes = null);
