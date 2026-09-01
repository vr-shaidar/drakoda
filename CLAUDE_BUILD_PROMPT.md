# Claude Code Master Build Prompt

Read `CLAUDE.md` first. Then read all relevant files under `docs/`, `progress.md`, and `decisions.md`.

You are the lead engineer responsible for implementing this project as a production-ready AI media generation SaaS.

## Goal
Build the complete platform described by the repository documentation.

It must support:
- AI image generation
- image editing/transformations where supported
- text-to-video
- image-to-video
- video transformation where supported
- audio/speech where supported
- OpenAI and Google integrations
- future providers through a provider adapter architecture
- model registry
- model capabilities
- dynamic provider/customer pricing
- credit wallet
- subscriptions
- pay-as-you-go credits
- Stripe billing
- usage metering
- projects/assets/history
- public API and API keys
- admin dashboard
- moderation
- security/rate limiting
- queues/workers
- storage
- retries/failover
- observability and tests

## Critical instruction
Do not attempt to implement everything in one giant unverified change.

Work in phases. At the beginning of each phase:
1. Read the relevant documentation.
2. Inspect the current code.
3. Identify dependencies.
4. Write a concise implementation plan.
5. Implement.
6. Run tests/build/lint.
7. Fix failures.
8. Update `progress.md`.
9. Update `decisions.md` if an architectural decision changed.
10. Only then continue.

## Phase 0 — Repository assessment
Inspect the repository before creating files.

If it is empty, create the documented solution structure.

Confirm:
- frontend project
- backend project
- shared contracts if needed
- database/migrations
- worker
- infrastructure
- tests
- documentation

Do not ask for confirmation for normal implementation decisions that are already covered by the docs.

## Phase 1 — Foundation
Implement:
- Next.js frontend shell
- .NET 8 backend
- PostgreSQL
- Redis
- Docker Compose
- configuration
- environment templates
- logging
- exception handling
- health endpoints
- OpenAPI
- database migrations
- testing infrastructure
- basic authentication/account structure

Do not implement provider integrations yet.

## Phase 2 — AI platform core
Implement:
- model registry
- capability system
- pricing engine interfaces
- AI Gateway
- provider adapter interfaces
- generation entities
- job entities
- queue/worker infrastructure
- normalized provider errors
- idempotency
- retry policy
- tests

The frontend must be able to retrieve available models/capabilities from the backend.

## Phase 3 — Provider integrations
Before coding each provider, inspect and verify the current official API documentation.

Implement:
1. Google adapter
2. OpenAI adapter

For each provider:
- authentication
- current supported models
- image capabilities
- video capabilities where available
- asynchronous behavior
- polling/webhooks where applicable
- request/response mapping
- error mapping
- usage/cost mapping
- mocked tests
- integration test hooks

Do not invent model IDs, endpoints or pricing.

## Phase 4 — Image generation
Implement the complete image workflow:
upload/input -> validation -> model selection -> price estimate -> credit reservation -> queue -> provider -> output storage -> billing finalization -> UI preview/download.

## Phase 5 — Video generation
Implement:
- text-to-video
- image-to-video
- video transformation only where supported

Use asynchronous jobs.
Show generation state in the UI.
Support cancellation where the provider supports it.
Store outputs in object storage.

## Phase 6 — Credits and pricing
Implement:
- wallet
- reservations
- captures
- releases
- refunds
- transaction ledger
- versioned pricing
- usage records
- customer price calculation
- provider cost tracking
- margin reporting

Financial operations must be transactional and idempotent.

## Phase 7 — Stripe
Implement:
- Stripe customer creation
- checkout
- subscription plans
- one-time credit packs
- payment events
- invoice events
- subscription lifecycle
- verified webhooks
- customer portal where appropriate

Never trust browser prices.

## Phase 8 — Projects/assets/history
Implement:
- projects
- folders/tags if appropriate
- assets
- generation history
- previews
- downloads
- signed URLs
- deletion/retention rules

## Phase 9 — Public API
Implement:
- API keys
- scoped permissions
- API authentication
- idempotency
- rate limits
- generation endpoints
- model catalog
- usage endpoints
- webhook callbacks
- developer documentation

## Phase 10 — Admin
Implement:
- users
- providers
- models
- capabilities
- pricing
- plans
- jobs
- failed generations
- usage
- billing
- credit adjustments
- audit logs
- system health

## Phase 11 — Security and moderation
Implement:
- upload validation
- authentication protections
- authorization
- rate limiting
- moderation pipeline
- webhook verification
- secret management
- audit logging
- abuse controls

## Phase 12 — Production readiness
Implement:
- production Docker configuration
- health/readiness checks
- worker scaling
- logging
- metrics
- error tracking hooks
- database backup guidance
- object storage lifecycle
- CI pipeline
- staging configuration
- production configuration templates

## Testing requirements
Critical tests must cover:
- registration/login
- model catalog
- capability filtering
- pricing calculation
- credit reservation/capture/release/refund
- duplicate webhook handling
- duplicate generation request/idempotency
- provider error mapping
- failed generation refunds
- successful generation billing
- API key authorization
- rate limiting
- admin authorization

## UI requirements
Build a polished modern AI creative interface.

The primary experience is an AI Studio:
- mode selector
- model selector
- prompt/input area
- dynamic model-specific controls
- source asset upload
- estimated credit cost
- generate button
- live generation status
- output preview
- download/save/share actions

Do not expose unsupported settings for a selected model.

## Provider pricing
Pricing must be verified from current official provider documentation before production configuration.
Never embed temporary guessed pricing into application logic.

## Important anti-patterns
Do NOT:
- call provider APIs from React
- hard-code model names throughout code
- hard-code prices in frontend
- store media blobs in PostgreSQL
- hold HTTP requests open for long video jobs
- trust client-submitted prices
- process Stripe webhooks without idempotency
- expose API secrets
- create fake provider responses and call the integration complete
- silently switch models without recording the fallback
- delete historical model/pricing references

## Final verification
Before declaring the project complete:
- run backend tests
- run frontend tests
- run integration tests
- run lint/type checks
- run production builds
- verify migrations from clean database
- verify Docker Compose startup
- verify critical billing flows
- verify generation flows with mocks
- verify provider integrations where credentials are available
- update progress.md

If something cannot be verified because credentials or an external service are unavailable, clearly mark it as requiring external verification rather than claiming it works.
