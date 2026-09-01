# Project Progress

## Current phase
Phase 4 — Pricing and Credits (in progress)

## Phases
- [x] Foundation — initial web/API/Docker/PostgreSQL/Redis/MinIO development foundation
- [ ] Identity/accounts
- [ ] AI Gateway — provider-neutral gateway, router and generation contracts added; real adapter execution pending
- [ ] Model registry — database entities and read API added; migrations/seed lifecycle still in progress
- [ ] Generation engine — durable generation entity, state machine, Redis queue and worker foundation added
- [ ] Google integration
- [ ] OpenAI integration
- [ ] Image generation
- [ ] Video generation
- [ ] Storage/assets — object-storage abstraction, project/asset persistence and asset service contract added
- [ ] Credits — immutable ledger domain and transactional service contract added
- [ ] Pricing engine — versioned pricing/rules domain and pricing service contract added
- [ ] Stripe billing
- [ ] Subscriptions
- [ ] Public API
- [ ] Admin
- [ ] Moderation/security
- [ ] Testing
- [ ] Production deployment

## Current task
Implement concrete storage/assets and pricing/credit services, then integrate them into generation execution.

## Completed
- Phase 1 web/API/container foundation committed on `phase-1-foundation`.
- PostgreSQL, Redis and MinIO development services defined.
- .NET 8 API configured with EF Core/Npgsql, Redis, ProblemDetails, CORS, health checks and Swagger/OpenAPI.
- Next.js TypeScript landing application added.
- Provider-neutral AI contracts and router exist; no provider API is faked.
- Database-driven provider/model registry entities and `GET /v1/models` added.
- Generation POST/GET/cancel endpoints added with idempotency lookup.
- Redis generation queue and background worker added.
- Generation state machine prevents invalid lifecycle transitions.
- AI Gateway abstraction added for submit/poll/cancel operations.
- Durable generation jobs/outputs, projects and assets persistence models added.
- Object storage abstraction added.
- Versioned pricing and immutable credit-ledger domain models added.

## Verification
- GitHub write access is working and implementation commits are being created on `phase-1-foundation`.
- This execution environment does not contain the Docker or .NET CLIs and cannot resolve external GitHub/Docker registry hosts, so builds, migrations and Compose startup cannot be executed here. REQUIRES EXTERNAL VERIFICATION.
- Real provider integrations require current official provider API verification and credentials before production verification.

## Known issues
- EF migrations need to be regenerated/validated with `dotnet ef` after the latest persistence-model changes.
- Concrete credit transaction implementation is not yet wired into generation execution.
- Queue delivery and dead-letter/retry behavior still require production hardening.

## Notes
Do not mark a phase complete until implementation and runtime verification have both been performed.
