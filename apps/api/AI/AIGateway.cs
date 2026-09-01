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
    private async Task<(AIModel Model, ProviderContext Context, IAIProviderAdapter Adapter)> ResolveAsync(Generation generation, CancellationToken ct)
    {
        var model = await db.AIModels.Include(x => x.Provider).SingleOrDefaultAsync(x => x.Id == generation.ModelId, ct) ?? throw new InvalidOperationException("MODEL_NOT_FOUND");
        if (!model.Enabled || model.Provider is not { Enabled: true }) throw new InvalidOperationException("MODEL_UNAVAILABLE");
        var settings = JsonSerializer.Deserialize<Dictionary<string, object?>>(generation.Settings) ?? new();
        var sources = JsonSerializer.Deserialize<List<string>>(generation.SourceAssetIds) ?? [];
        var adapter = router.ResolveFor(model.Provider.Key, model.ExternalModelId, generation.Mode);
        var context = new ProviderContext(generation.Id, model.ExternalModelId, generation.Mode, generation.Prompt, settings, sources, generation.ExternalJobId);
        return (model, context, adapter);
    }

    public async Task<ProviderSubmission> SubmitAsync(Generation generation, CancellationToken cancellationToken)
    {
        var (_, context, adapter) = await ResolveAsync(generation, cancellationToken);
        return await adapter.SubmitAsync(context, cancellationToken);
    }

    public async Task<ProviderResult> PollAsync(Generation generation, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(generation.ExternalJobId)) throw new InvalidOperationException("EXTERNAL_JOB_NOT_FOUND");
        var (_, context, adapter) = await ResolveAsync(generation, cancellationToken);
        return await adapter.GetResultAsync(context, cancellationToken);
    }

    public async Task CancelAsync(Generation generation, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(generation.ExternalJobId)) return;
        var (_, context, adapter) = await ResolveAsync(generation, cancellationToken);
        await adapter.CancelAsync(context, cancellationToken);
    }
}
