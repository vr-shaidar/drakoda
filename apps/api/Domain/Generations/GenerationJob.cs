namespace Drakoda.Api.Domain.Generations;

public sealed class GenerationJob
{
    public Guid Id { get; set; }
    public Guid GenerationId { get; set; }
    public string QueueMessageId { get; set; } = string.Empty;
    public int Attempt { get; set; }
    public string? ProviderJobId { get; set; }
    public DateTimeOffset? LeaseUntil { get; set; }
    public DateTimeOffset? NextAttemptAt { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public string? LastErrorCode { get; set; }
    public string? LastErrorMessage { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class GenerationOutput
{
    public Guid Id { get; set; }
    public Guid GenerationId { get; set; }
    public string MediaType { get; set; } = string.Empty;
    public string StorageUri { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public long? SizeBytes { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public double? DurationSeconds { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
