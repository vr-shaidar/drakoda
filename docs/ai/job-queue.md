# Job Queue

Generation jobs must be durable and retryable.

Required fields:
generationId, status, attempts, nextAttemptAt, providerJobId, lockedAt, startedAt, completedAt, lastError.

Use idempotent worker handling.
Use bounded retries and exponential backoff for transient provider/network errors.
Dead-letter permanently failing jobs for admin inspection.
