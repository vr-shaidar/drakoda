using Drakoda.AI;
using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Domain.Generations;

public interface IGenerationJobService
{
    Task ExecuteAsync(Guid generationId, CancellationToken cancellationToken);
}

public sealed class GenerationJobService(DrakodaDbContext db, ILogger<GenerationJobService> logger) : IGenerationJobService
{
    public async Task ExecuteAsync(Guid generationId, CancellationToken cancellationToken)
    {
        var generation = await db.Generations.SingleOrDefaultAsync(x => x.Id == generationId, cancellationToken);
        if (generation is null) return;

        if (generation.Status == GenerationStatus.Validating)
        {
            if (string.IsNullOrWhiteSpace(generation.Prompt) || generation.Prompt.Length > 10000)
            {
                GenerationStateMachine.Transition(generation, GenerationStatus.ValidationFailed);
                generation.ErrorCode = "INVALID_PROMPT";
                generation.ErrorMessage = "Prompt is required and must not exceed 10,000 characters.";
                await db.SaveChangesAsync(cancellationToken);
                return;
            }
            GenerationStateMachine.Transition(generation, GenerationStatus.Moderation);
        }

        if (generation.Status == GenerationStatus.Moderation)
            GenerationStateMachine.Transition(generation, GenerationStatus.CostEstimated);

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Generation {GenerationId} advanced to {Status}.", generationId, generation.Status);
    }
}
