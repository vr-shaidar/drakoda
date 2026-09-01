using Drakoda.AI;
using Microsoft.AspNetCore.Mvc;

namespace Drakoda.Api.Controllers;

[ApiController]
[Route("v1/models")]
public sealed class ModelsController(AIModelRegistry registry) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] MediaType? mediaType, CancellationToken ct)
    {
        var models = await registry.GetEnabledAsync(mediaType, ct);
        return Ok(models.Select(x => new
        {
            id = x.Id,
            provider = x.Provider!.Key,
            externalModelId = x.ExternalModelId,
            displayName = x.DisplayName,
            mediaType = x.MediaType.ToString().ToLowerInvariant(),
            capabilities = x.Capabilities,
            metadata = x.Metadata,
            priority = x.Priority,
            maxConcurrency = x.MaxConcurrency
        }));
    }
}
