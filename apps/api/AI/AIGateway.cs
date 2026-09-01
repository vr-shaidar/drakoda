using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Drakoda.AI;

public interface IAIGateway
{
    Task<ProviderSubmission> SubmitAsync(Generation generation, CancellationToken cancellationToken);
    Task<ProviderResult> PollAsync(Generation generation, CancellationToken cancellationToken);
    Task CancelAsync(Generation generation, CancellationToken cancellationToken);
}

public sealed class AIGateway(DrakodaDbContext db, IProviderRouter router) : IAIGateway
{
    public async Task<ProviderSubmission> SubmitAsync(Generation generation, CancellationToken cancellationToken)
    {
        var model = await db.AIModels.Include(x => x.Provider).SingleOrDefaultAsync(x => x.Id == generation.ModelId, cancellationToken)
            ?? throw new InvalidOperationException("MODEL_NOT_FOUND");
        if (!model.Enabled || model.Provider is not { Enabled: true }) throw new InvalidOperationException("MODEL_UNAVAILABLE");

        var settings = JsonSerializer.Deserialize<Dictionary<string, object?>>(generation.Settings) ?? new();
        var sources = JsonSerializer.Deserialize<List<string>>(generation.SourceAssetIds) ?? [];
        var adapter = router.ResolveFor(model.Provider.Key, model.ExternalModelId, generation.Mode);
        var context = new ProviderContext(generation.Id, model.ExternalModelId, generation.Mode, generation.Prompt, settings, sources);
        return await adapter.SubmitAsync(context, cancellationToken);
    }

    public async Task<ProviderResult> PollAsync(Generation generation, CancellationToken cancellationToken)
    {
        var model = await db.AIModels.Include(x => x.Provider).SingleOrDefaultAsync(x => x.Id == generation.ModelId, cancellationToken)
            ?? throw new InvalidOperationException("MODEL_NOT_FOUND");
        if (string.IsNullOrWhiteSpace(generation.ExternalJobId)) throw new InvalidOperationException("EXTERNAL_JOB_NOT_FOUND");
        var adapter = router.ResolveFor(model.Provider!.Key, model.ExternalModelId, generation.Mode);
        var settings = JsonSerializer.Deserialize<Dictionary<string, object?>>(generation.Settings) ?? new();
        var sources = JsonSerializer.Deserialize<List<string>>(generation.SourceAssetIds) ?? [];
        return await adapter.GetResultAsync(new ProviderContext(generation.Id, model.ExternalModelId, generation.Mode, generation.Prompt, settings, sources), cancellationToken);
    }

    public async Task CancelAsync(Generation generation, CancellationToken cancellationToken)
    {
        var model = await db.AIModels.Include(x => x.Provider).SingleOrDefaultAsync(x => x.Id == generation.ModelId, cancellationToken)
            ?? throw new InvalidOperationException("MODEL_NOT_FOUND");
        if (string.IsNullOrWhiteSpace(generation.ExternalJobId)) return;
        var adapter = router.ResolveFor(model.Provider!.Key, model.ExternalModelId, generation.Mode);
        var settings = JsonSerializer.Deserialize<Dictionary<string, object?>>(generation.Settings) ?? new();
        var sources = JsonSerializer.Deserialize<List<string>>(generation.SourceAssetIds) ?? [];
        await adapter.CancelAsync(new ProviderContext(generation.Id, model.ExternalModelId, generation.Mode, generation.Prompt, settings, sources), cancellationToken);
    }
}
