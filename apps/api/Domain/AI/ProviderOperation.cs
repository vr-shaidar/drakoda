namespace Drakoda.Api.Domain.AI;

public enum ProviderOperationState { Submitted, Processing, Completed, Failed, Cancelled }

public sealed record ProviderOperationResult(
    ProviderOperationState State,
    string? ExternalJobId,
    IReadOnlyList<ProviderOutput> Outputs,
    string? ErrorCode = null,
    string? ErrorMessage = null);

public sealed record ProviderOutput(
    string MediaType,
    string ContentType,
    Stream Content,
    string? FileName = null);

public interface IProviderOperationClient
{
    Task<ProviderOperationResult> GetStatusAsync(string externalJobId, CancellationToken cancellationToken = default);
}
