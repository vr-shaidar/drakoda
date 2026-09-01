# Project Progress

## Current phase
Phase 2 — AI Core (in progress)

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
- [ ] Storage/assets
- [ ] Credits
- [ ] Pricing engine
- [ ] Stripe billing
- [ ] Subscriptions
- [ ] Public API
- [ ] Admin
- [ ] Moderation/security
- [ ] Testing
- [ ] Production deployment

## Current task
Complete AI Core execution orchestration, then move to storage/assets and billing foundations.

## Completed
- Phase 1 web/API/container foundation committed on `phase-1-foundation`.
- PostgreSQL, Redis and MinIO development services defined.
- .NET 8 API configured with EF Core/Npgsql, Redis, ProblemDetails, CORS, health checks and Swagger/OpenAPI.
- Next.js TypeScript landing application added.
- EF migrations/snapshot now include provider/model and generation tables.
- Provider-neutral AI contracts and router exist; no provider API is faked.
- Database-driven provider/model registry entities and `GET /v1/models` added.
- Generation POST/GET/cancel endpoints added with idempotency lookup.
- Redis generation queue and background worker added.
- Generation state machine prevents invalid lifecycle transitions.
- AI Gateway abstraction added for submit/poll/cancel operations.
- Unit-test project and initial generation state-machine tests added.

## Verification
- GitHub write access is working and implementation commits are being created on `phase-1-foundation`.
- This execution environment does not contain the Docker or .NET CLIs and cannot resolve external GitHub/Docker registry hosts, so builds, migrations and Compose startup cannot be executed here. REQUIRES EXTERNAL VERIFICATION.
- Real provider integrations remain intentionally pending official current API verification.

## Known issues
- The model registry and generation migrations are handwritten because the .NET CLI is unavailable in this execution environment; they must be validated with `dotnet ef` before release.
- Authentication, credits and pricing are not yet implemented, so generation submission is not production-billable yet.
- Queue delivery is currently Redis-list based; a durable retry/dead-letter strategy will be hardened before production.

## Notes
Do not mark a phase complete until implementation and runtime verification have both been performed.
