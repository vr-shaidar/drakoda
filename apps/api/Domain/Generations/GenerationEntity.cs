using Drakoda.AI;

namespace Drakoda.AI;

public sealed class Generation
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public Guid? UserId { get; set; }
    public GenerationMode Mode { get; set; }
    public GenerationStatus Status { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string Settings { get; set; } = "{}";
    public string SourceAssetIds { get; set; } = "[]";
    public string? IdempotencyKey { get; set; }
    public string? ExternalJobId { get; set; }
    public string? ProviderRequestId { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public int AttemptCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}
