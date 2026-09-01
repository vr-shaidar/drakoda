using Drakoda.AI;

namespace Drakoda.Api.Domain.Generations;

public static class GenerationStateMachine
{
    private static readonly IReadOnlyDictionary<GenerationStatus, GenerationStatus[]> Allowed =
        new Dictionary<GenerationStatus, GenerationStatus[]>
        {
            [GenerationStatus.Requested] = [GenerationStatus.Validating, GenerationStatus.Cancelled],
            [GenerationStatus.Validating] = [GenerationStatus.Moderation, GenerationStatus.ValidationFailed, GenerationStatus.ModerationFailed],
            [GenerationStatus.Moderation] = [GenerationStatus.CostEstimated, GenerationStatus.ModerationFailed],
            [GenerationStatus.CostEstimated] = [GenerationStatus.CreditsReserved, GenerationStatus.BillingFailed],
            [GenerationStatus.CreditsReserved] = [GenerationStatus.Queued, GenerationStatus.BillingFailed],
            [GenerationStatus.Queued] = [GenerationStatus.Submitted, GenerationStatus.Cancelled, GenerationStatus.ProviderFailed],
            [GenerationStatus.Submitted] = [GenerationStatus.Processing, GenerationStatus.Completed, GenerationStatus.ProviderFailed, GenerationStatus.Timeout],
            [GenerationStatus.Processing] = [GenerationStatus.Downloading, GenerationStatus.Completed, GenerationStatus.ProviderFailed, GenerationStatus.Timeout, GenerationStatus.Cancelled],
            [GenerationStatus.Downloading] = [GenerationStatus.Storing, GenerationStatus.ProviderFailed, GenerationStatus.Timeout],
            [GenerationStatus.Storing] = [GenerationStatus.Completed, GenerationStatus.ProviderFailed],
        };

    public static bool CanTransition(GenerationStatus from, GenerationStatus to) =>
        from == to || (Allowed.TryGetValue(from, out var targets) && targets.Contains(to));

    public static void Transition(Generation generation, GenerationStatus target)
    {
        if (!CanTransition(generation.Status, target))
            throw new InvalidOperationException($"Invalid generation transition: {generation.Status} -> {target}.");
        generation.Status = target;
        generation.UpdatedAt = DateTimeOffset.UtcNow;
        if (target == GenerationStatus.Completed) generation.CompletedAt = DateTimeOffset.UtcNow;
    }
}
