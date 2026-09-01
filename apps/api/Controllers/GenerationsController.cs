using System.Text.Json;
using Drakoda.AI;
using Drakoda.Api.Domain.Generations;
using Drakoda.Api.Infrastructure.Queue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Controllers;

[ApiController]
[Route("v1/generations")]
public sealed class GenerationsController(DrakodaDbContext db, IGenerationQueue queue, IAIGateway gateway) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateGenerationDto request, CancellationToken cancellationToken)
    {
        if (request.ModelId == Guid.Empty || string.IsNullOrWhiteSpace(request.Prompt))
            return ValidationProblem("modelId and prompt are required.");

        var idempotencyKey = Request.Headers.IdempotencyKey.ToString();
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var existing = await db.Generations.FirstOrDefaultAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
            if (existing is not null) return Ok(new { id = existing.Id, status = existing.Status.ToString(), idempotent = true });
        }

        var model = await db.AIModels.FirstOrDefaultAsync(x => x.Id == request.ModelId && x.Enabled && x.Provider != null && x.Provider.Enabled, cancellationToken);
        if (model is null) return NotFound(new { error = "MODEL_NOT_FOUND" });

        var now = DateTimeOffset.UtcNow;
        var generation = new Generation
        {
            Id = Guid.NewGuid(), ModelId = request.ModelId, Mode = request.Mode,
            Status = GenerationStatus.Requested, Prompt = request.Prompt,
            Settings = JsonSerializer.Serialize(request.Settings ?? new()),
            SourceAssetIds = JsonSerializer.Serialize(request.SourceAssetIds ?? []),
            IdempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey,
            CreatedAt = now, UpdatedAt = now
        };

        db.Generations.Add(generation);
        GenerationStateMachine.Transition(generation, GenerationStatus.Validating);
        await db.SaveChangesAsync(cancellationToken);
        await queue.EnqueueAsync(generation.Id, cancellationToken);

        return Accepted($"/v1/generations/{generation.Id}", new { id = generation.Id, status = generation.Status.ToString() });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var generation = await db.Generations.FindAsync([id], cancellationToken);
        return generation is null ? NotFound() : Ok(generation);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var generation = await db.Generations.FindAsync([id], cancellationToken);
        if (generation is null) return NotFound();
        if (generation.Status is GenerationStatus.Completed or GenerationStatus.Cancelled or GenerationStatus.ValidationFailed or GenerationStatus.ModerationFailed or GenerationStatus.ProviderFailed)
            return Conflict(new { error = "GENERATION_NOT_CANCELLABLE", status = generation.Status.ToString() });

        if (generation.Status is GenerationStatus.Submitted or GenerationStatus.Processing)
            await gateway.CancelAsync(generation, cancellationToken);

        GenerationStateMachine.Transition(generation, GenerationStatus.Cancelled);
        await db.SaveChangesAsync(cancellationToken);
        return Ok(new { id = generation.Id, status = generation.Status.ToString() });
    }
}

public sealed record CreateGenerationDto(Guid ModelId, GenerationMode Mode, string Prompt, Dictionary<string, object?>? Settings, List<Guid>? SourceAssetIds);
