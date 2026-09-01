using Drakoda.Api.Domain.Billing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Controllers;

[ApiController]
[Route("v1/credits")]
public sealed class CreditsController(DrakodaDbContext db, CreditLedgerService ledger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var userId = ResolveUserId();
        var wallet = await ledger.GetOrCreateWalletAsync(userId, ct);
        return Ok(new { wallet.Id, wallet.AvailableCredits, wallet.ReservedCredits, wallet.UpdatedAt });
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> Transactions(CancellationToken ct)
    {
        var userId = ResolveUserId();
        var wallet = await db.CreditWallets.SingleOrDefaultAsync(x => x.UserId == userId, ct);
        if (wallet is null) return Ok(Array.Empty<object>());
        return Ok(await db.CreditTransactions.Where(x => x.WalletId == wallet.Id).OrderByDescending(x => x.CreatedAt).Take(100).ToListAsync(ct));
    }

    private Guid ResolveUserId() => Guid.TryParse(User.FindFirst("sub")?.Value, out var id) ? id : Guid.Empty;
}
