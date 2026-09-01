# Refund Rules

If a generation fails before provider execution, release reserved credits.

If provider execution fails and no usable output is produced, refund according to configured policy.

If output succeeds, capture actual billable credits.

Manual admin refunds must create immutable compensating credit transactions and audit logs.
