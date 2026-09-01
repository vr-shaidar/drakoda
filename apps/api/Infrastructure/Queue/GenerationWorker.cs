using Drakoda.AI;
using Drakoda.Api.Domain.Generations;
using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Infrastructure.Queue;

public sealed class GenerationWorker(IGenerationQueue queue, IServiceScopeFactory scopeFactory, ILogger<GenerationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Guid id;
            try { id = await queue.DequeueAsync(stoppingToken); }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }

            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DrakodaDbContext>();
                var generation = await db.Generations.FirstOrDefaultAsync(x => x.Id == id, stoppingToken);
                if (generation is null) continue;

                if (generation.Status == GenerationStatus.Validating)
                    GenerationStateMachine.Transition(generation, GenerationStatus.Moderation);
                if (generation.Status == GenerationStatus.Moderation)
                    GenerationStateMachine.Transition(generation, GenerationStatus.CostEstimated);

                await db.SaveChangesAsync(stoppingToken);
                logger.LogInformation("Generation {GenerationId} reached {Status}.", id, generation.Status);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Generation worker failed while processing {GenerationId}.", id);
            }
        }
    }
}
