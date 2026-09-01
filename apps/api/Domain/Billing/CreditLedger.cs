namespace Drakoda.Api.Domain.Billing;

public enum CreditTransactionType { Grant, Purchase, Reserve, Capture, Release, Refund, Adjustment, Expiration }

public sealed class CreditWallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal AvailableCredits { get; set; }
    public decimal ReservedCredits { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class CreditTransaction
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public CreditTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public Guid? GenerationId { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
