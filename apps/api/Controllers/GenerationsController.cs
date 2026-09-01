using System.Text.Json;
using Drakoda.Api.Domain.AI;
using Drakoda.Api.Domain.Generations;
using Drakoda.Api.Infrastructure.Queue;
using Microsoft.AspNetCore.Mvc;

namespace Drakoda.Api.Controllers;

[ApiController]
[Route("v1/generations")]
public sealed class GenerationsController(DrakodaDbContext db, IGenerationQueue queue) : ControllerBase
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

        var now = DateTimeOffset.UtcNow;
        var generation = new Generation
        {
            Id = Guid.NewGuid(), ModelId = request.ModelId, MediaType = request.MediaType,
            Status = GenerationStatus.Requested, Prompt = request.Prompt,
            SettingsJson = JsonSerializer.Serialize(request.Settings ?? new()),
            SourceAssetIdsJson = JsonSerializer.Serialize(request.SourceAssetIds ?? []),
            IdempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey,
            CreatedAt = now, UpdatedAt = now
        };

        db.Generations.Add(generation);
        await db.SaveChangesAsync(cancellationToken);
        await queue.EnqueueAsync(generation.Id, cancellationToken);
        GenerationStateMachine.Transition(generation, GenerationStatus.Validating);
        await db.SaveChangesAsync(cancellationToken);

        return Accepted($"/v1/generations/{generation.Id}", new { id = generation.Id, status = generation.Status.ToString() });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var generation = await db.Generations.FindAsync([id], cancellationToken);
        return generation is null ? NotFound() : Ok(generation);
    }
}

public sealed record CreateGenerationDto(Guid ModelId, MediaType MediaType, string Prompt, Dictionary<string, object?>? Settings, List<Guid>? SourceAssetIds);
