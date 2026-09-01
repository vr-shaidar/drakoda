namespace Drakoda.Api.Domain.Billing;

public interface IPricingService
{
    Task<PriceEstimate> EstimateAsync(Guid modelId, PricingContext context, CancellationToken cancellationToken);
}

public sealed record PricingContext(decimal OutputCount = 1, decimal? Megapixels = null, decimal? VideoSeconds = null, decimal? AudioSeconds = null, decimal? InputTokens = null, decimal? OutputTokens = null, decimal? Characters = null, string? Quality = null, string? Resolution = null);
public sealed record PriceEstimate(Guid PricingVersionId, decimal Credits, string Currency);
