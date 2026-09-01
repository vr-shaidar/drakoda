using System.Security.Claims;
using Drakoda.Api.Domain.Assets;
using Drakoda.Api.Domain.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Controllers;

[ApiController]
[Route("v1/assets")]
public sealed class AssetsController(DrakodaDbContext db, AssetService assets, IObjectStorage storage) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(524_288_000)]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] Guid? projectId, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        if (file is null || file.Length == 0) return ValidationProblem("A non-empty file is required.");
        await using var stream = file.OpenReadStream();
        var asset = await assets.UploadAsync(userId, stream, file.FileName, file.ContentType, file.Length, projectId, ct);
        return Created($"/v1/assets/{asset.Id}", new { asset.Id, asset.FileName, asset.ContentType, asset.SizeBytes, asset.ProjectId });
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] Guid? projectId, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var query = db.Assets.Where(x => x.UserId == userId);
        if (projectId.HasValue) query = query.Where(x => x.ProjectId == projectId);
        return Ok(await query.OrderByDescending(x => x.CreatedAt).ToListAsync(ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var asset = await db.Assets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        return asset is null ? NotFound() : Ok(asset);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var asset = await db.Assets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (asset is null) return NotFound();
        var stream = await storage.GetAsync(asset.StorageKey, ct);
        return File(stream, asset.ContentType, asset.FileName, enableRangeProcessing: true);
    }

    private bool TryGetUserId(out Guid userId) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"), out userId);
}
