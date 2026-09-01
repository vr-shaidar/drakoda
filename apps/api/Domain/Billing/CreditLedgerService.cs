using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Domain.Billing;

public sealed class CreditLedgerService(DrakodaDbContext db)
{
    public async Task<CreditWallet> GetOrCreateWalletAsync(Guid userId, CancellationToken ct)
    {
        var wallet = await db.CreditWallets.SingleOrDefaultAsync(x => x.UserId == userId, ct);
        if (wallet is not null) return wallet;
        wallet = new CreditWallet { Id = Guid.NewGuid(), UserId = userId, UpdatedAt = DateTimeOffset.UtcNow };
        db.CreditWallets.Add(wallet); await db.SaveChangesAsync(ct); return wallet;
    }

    public async Task<CreditTransaction> AppendAsync(Guid walletId, CreditTransactionType type, decimal amount, Guid? generationId, string idempotencyKey, string? description, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) throw new ArgumentException("Idempotency key is required.", nameof(idempotencyKey));
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        var existing = await db.CreditTransactions.SingleOrDefaultAsync(x => x.WalletId == walletId && x.IdempotencyKey == idempotencyKey, ct);
        if (existing is not null) return existing;
        var wallet = await db.CreditWallets.SingleAsync(x => x.Id == walletId, ct);
        if (amount == 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (type is CreditTransactionType.Reserve && wallet.AvailableCredits < amount) throw new InvalidOperationException("INSUFFICIENT_CREDITS");
        if (type is CreditTransactionType.Capture or CreditTransactionType.Release && wallet.ReservedCredits < amount) throw new InvalidOperationException("INSUFFICIENT_RESERVED_CREDITS");
        switch (type)
        {
            case CreditTransactionType.Reserve: wallet.AvailableCredits -= amount; wallet.ReservedCredits += amount; break;
            case CreditTransactionType.Capture: wallet.ReservedCredits -= amount; break;
            case CreditTransactionType.Release: wallet.ReservedCredits -= amount; wallet.AvailableCredits += amount; break;
            case CreditTransactionType.Grant: case CreditTransactionType.Purchase: case CreditTransactionType.Refund: case CreditTransactionType.Adjustment: wallet.AvailableCredits += amount; break;
            case CreditTransactionType.Expiration: wallet.AvailableCredits -= amount; break;
        }
        if (wallet.AvailableCredits < 0 || wallet.ReservedCredits < 0) throw new InvalidOperationException("CREDIT_BALANCE_INVALID");
        wallet.UpdatedAt = DateTimeOffset.UtcNow;
        var entry = new CreditTransaction { Id = Guid.NewGuid(), WalletId = walletId, Type = type, Amount = amount, BalanceAfter = wallet.AvailableCredits, GenerationId = generationId, IdempotencyKey = idempotencyKey, Description = description, CreatedAt = DateTimeOffset.UtcNow };
        db.CreditTransactions.Add(entry); await db.SaveChangesAsync(ct); await tx.CommitAsync(ct); return entry;
    }
}
