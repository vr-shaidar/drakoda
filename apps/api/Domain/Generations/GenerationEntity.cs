using Drakoda.Api.Domain.AI;

namespace Drakoda.Api.Domain.Generations;

public sealed class Generation
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public Guid? UserId { get; set; }
    public MediaType MediaType { get; set; }
    public GenerationStatus Status { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string SettingsJson { get; set; } = "{}";
    public string SourceAssetIdsJson { get; set; } = "[]";
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
