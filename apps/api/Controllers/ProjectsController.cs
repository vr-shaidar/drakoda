using Drakoda.Api.Domain.Assets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Drakoda.Api.Controllers;

[ApiController]
[Route("v1/projects")]
public sealed class ProjectsController(DrakodaDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        return Ok(await db.Projects.Where(x => x.UserId == userId).OrderByDescending(x => x.UpdatedAt).ToListAsync(ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200) return ValidationProblem("Project name is required and must not exceed 200 characters.");
        var now = DateTimeOffset.UtcNow;
        var project = new Project { Id = Guid.NewGuid(), UserId = userId, Name = request.Name.Trim(), CreatedAt = now, UpdatedAt = now };
        db.Projects.Add(project); await db.SaveChangesAsync(ct);
        return Created($"/v1/projects/{project.Id}", project);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (project is null) return NotFound();
        db.Projects.Remove(project); await db.SaveChangesAsync(ct); return NoContent();
    }

    private bool TryGetUserId(out Guid userId) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"), out userId);
}

public sealed record CreateProjectRequest(string Name);
