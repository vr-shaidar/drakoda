namespace Drakoda.Api.Domain.Billing;

public interface ICreditService
{
    Task<CreditReservation> ReserveAsync(Guid userId, decimal amount, Guid generationId, string idempotencyKey, CancellationToken cancellationToken);
    Task CaptureAsync(Guid reservationId, decimal actualAmount, CancellationToken cancellationToken);
    Task ReleaseAsync(Guid reservationId, CancellationToken cancellationToken);
    Task RefundAsync(Guid userId, decimal amount, Guid generationId, string idempotencyKey, CancellationToken cancellationToken);
}

public sealed record CreditReservation(Guid Id, Guid WalletId, decimal Amount, decimal AvailableAfter);
