# System Architecture

## High-level
Browser -> Next.js -> .NET API -> application services -> AI Gateway -> provider adapters -> AI providers.

Supporting systems:
PostgreSQL for durable state, Redis for caching/rate limits/job coordination, object storage for media, CDN for delivery, Stripe for billing.

## AI Gateway
Application code calls capabilities such as:
GenerateImage, GenerateVideo, ImageToVideo, TransformVideo, GenerateAudio, EnhancePrompt, GetJobStatus, CancelJob, EstimateCost.

The gateway resolves:
request -> model registry -> capability validation -> pricing -> provider router -> adapter.

## Async generation
POST request creates a generation record and job.
Worker executes the provider request.
Provider completion is received by webhook or polling.
Output is downloaded/stored.
Generation becomes COMPLETED or FAILED.
Credits are captured/refunded according to billing rules.

## Reliability
Use idempotency keys for generation creation, payment webhooks and provider callbacks.
Retry transient failures with bounded exponential backoff.
Do not retry non-retryable validation/moderation failures.
Provider failover must respect capability and customer-plan constraints.

## Modular monolith
Keep modules separated by boundaries:
Identity, Catalog, Generation, Billing, Storage, Developer API, Admin.
Do not introduce microservices unless a measured scalability requirement justifies it.
