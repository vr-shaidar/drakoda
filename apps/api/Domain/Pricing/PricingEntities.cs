namespace Drakoda.Api.Domain.Pricing;

public sealed class ModelPricing
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public int Version { get; set; }
    public decimal ProviderCostPerUnit { get; set; }
    public decimal CustomerPricePerUnit { get; set; }
    public PricingUnit Unit { get; set; }
    public decimal MinimumCharge { get; set; }
    public decimal Multiplier { get; set; } = 1m;
    public DateTimeOffset EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
    public bool IsActive { get; set; } = true;
}
