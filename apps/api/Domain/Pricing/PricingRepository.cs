using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Domain.Pricing;

public sealed class PricingRepository(DrakodaDbContext db)
{
    public Task<List<ModelPricing>> GetEffectiveAsync(Guid modelId, DateTimeOffset at, CancellationToken ct) =>
        db.ModelPricing.Where(x => x.ModelId == modelId && x.IsActive && x.EffectiveFrom <= at && (x.EffectiveTo == null || x.EffectiveTo > at)).OrderBy(x => x.Version).ToListAsync(ct);

    public async Task<ModelPricing> AddVersionAsync(ModelPricing pricing, CancellationToken ct)
    {
        var current = await db.ModelPricing.Where(x => x.ModelId == pricing.ModelId && x.IsActive && x.EffectiveTo == null).ToListAsync(ct);
        foreach (var item in current) item.EffectiveTo = pricing.EffectiveFrom;
        db.ModelPricing.Add(pricing); await db.SaveChangesAsync(ct); return pricing;
    }
}
