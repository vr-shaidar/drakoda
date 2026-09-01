namespace Drakoda.Api.Domain.Billing;

public enum PricingUnit { Request, Image, Megapixel, Resolution, Quality, VideoSecond, AudioSecond, Token, Character, Output }

public sealed class PricingVersion
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset EffectiveFrom { get; set; }
    public DateTimeOffset? EffectiveTo { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal CustomerCreditRate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class PricingRule
{
    public Guid Id { get; set; }
    public Guid PricingVersionId { get; set; }
    public PricingUnit Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitSize { get; set; } = 1;
    public string? ConditionsJson { get; set; }
}
