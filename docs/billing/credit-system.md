# Credit System

Use integer credits.

Wallet:
balance, reservedBalance, availableBalance.

Reservation flow:
available -> reserved -> captured
or
reserved -> released/refunded.

Use database transactions and idempotency keys.

Example conversion:
customer price / credit unit price = credits.
Round according to a single documented policy.

Never allow negative available balance unless an explicit credit/overdraft feature exists.
