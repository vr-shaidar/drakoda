namespace Drakoda.Api.Domain.Pricing;

public enum PricingUnit { Request, Image, Megapixel, Resolution, Quality, VideoSecond, AudioSecond, InputToken, OutputToken, Character, Output }

public sealed record PricingRule(Guid Id, Guid ModelId, int Version, PricingUnit Unit, decimal UnitPrice, decimal MinimumCharge, decimal Multiplier, DateTimeOffset EffectiveFrom, DateTimeOffset? EffectiveTo);
public sealed record UsageMetrics(decimal Requests = 0, decimal Images = 0, decimal Megapixels = 0, decimal VideoSeconds = 0, decimal AudioSeconds = 0, decimal InputTokens = 0, decimal OutputTokens = 0, decimal Characters = 0, decimal Outputs = 0);
public sealed record PriceQuote(decimal ProviderCost, decimal CustomerPrice, int PricingVersion, IReadOnlyDictionary<string, decimal> Components);

public interface IPricingEngine
{
    PriceQuote Calculate(IEnumerable<PricingRule> rules, UsageMetrics usage);
}

public sealed class PricingEngine : IPricingEngine
{
    public PriceQuote Calculate(IEnumerable<PricingRule> rules, UsageMetrics usage)
    {
        var components = new Dictionary<string, decimal>();
        var version = rules.Select(x => x.Version).DefaultIfEmpty(0).Max();
        foreach (var rule in rules.Where(x => x.EffectiveFrom <= DateTimeOffset.UtcNow && (x.EffectiveTo == null || x.EffectiveTo > DateTimeOffset.UtcNow)))
        {
            var quantity = rule.Unit switch
            {
                PricingUnit.Request => usage.Requests,
                PricingUnit.Image => usage.Images,
                PricingUnit.Megapixel => usage.Megapixels,
                PricingUnit.VideoSecond => usage.VideoSeconds,
                PricingUnit.AudioSecond => usage.AudioSeconds,
                PricingUnit.InputToken => usage.InputTokens,
                PricingUnit.OutputToken => usage.OutputTokens,
                PricingUnit.Character => usage.Characters,
                PricingUnit.Output => usage.Outputs,
                _ => 0
            };
            var amount = Math.Max(rule.MinimumCharge, quantity * rule.UnitPrice * rule.Multiplier);
            components[$"{rule.Unit}:{rule.Id}"] = amount;
        }
        return new PriceQuote(components.Values.Sum(), components.Values.Sum(), version, components);
    }
}
