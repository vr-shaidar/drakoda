using Microsoft.EntityFrameworkCore;

namespace Drakoda.AI;

public sealed class AIProvider
{
    public Guid Id { get; set; }
    public required string Key { get; set; }
    public required string DisplayName { get; set; }
    public bool Enabled { get; set; }
}

public sealed class AIModel
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public required string ExternalModelId { get; set; }
    public required string DisplayName { get; set; }
    public MediaType MediaType { get; set; }
    public bool Enabled { get; set; }
    public int Priority { get; set; }
    public int MaxConcurrency { get; set; }
    public string Capabilities { get; set; } = "[]";
    public string Metadata { get; set; } = "{}";
    public AIProvider? Provider { get; set; }
}

public sealed class AIModelRegistry(DrakodaDbContext db)
{
    public Task<List<AIModel>> GetEnabledAsync(MediaType? mediaType, CancellationToken ct) =>
        db.Set<AIModel>().Include(x => x.Provider)
          .Where(x => x.Enabled && x.Provider != null && x.Provider.Enabled && (mediaType == null || x.MediaType == mediaType))
          .OrderBy(x => x.Priority).ThenBy(x => x.DisplayName).ToListAsync(ct);
}
