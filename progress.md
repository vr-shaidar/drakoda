# Project Progress

## Current phase
Phase 2 — AI Core (in progress)

## Phases
- [x] Foundation — initial web/API/Docker/PostgreSQL/Redis/MinIO development foundation
- [ ] Identity/accounts
- [ ] AI Gateway — contracts/router foundation added; execution workflow still in progress
- [ ] Model registry — database entities and read API added; migration/seeding still in progress
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
Build durable generation entities, queue/worker orchestration, idempotency and normalized AI Gateway execution.

## Completed
- Phase 1 web/API/container foundation committed on `phase-1-foundation`.
- PostgreSQL, Redis and MinIO development services defined.
- .NET 8 API configured with EF Core/Npgsql, Redis, ProblemDetails, CORS, health checks and Swagger/OpenAPI.
- Next.js TypeScript landing application added.
- Initial EF migration and model snapshot added.
- Provider-neutral AI contracts and router added; no provider API is faked.
- Database-driven provider/model registry entities and `GET /v1/models` added.

## Verification
- GitHub write access is working and implementation commits are being created on `phase-1-foundation`.
- This execution environment does not contain the Docker or .NET CLIs and cannot resolve external GitHub/Docker registry hosts, so builds, migrations and Compose startup cannot be executed here. REQUIRES EXTERNAL VERIFICATION.
- Real provider integrations remain intentionally pending official current API verification.

## Known issues
- The model registry schema needs its follow-up EF migration before `AIModelRegistry` can be used against a fresh database.
- Authentication is not yet implemented; the public model endpoint is currently an architectural foundation and must be protected according to the API/security specification as authenticated APIs are added.

## Notes
Do not mark a phase complete until implementation and runtime verification have both been performed.
