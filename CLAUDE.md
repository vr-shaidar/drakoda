# AI Media Generation SaaS — Claude Code Instructions

## Mission
Build a production-ready SaaS platform for AI image, video and audio generation.
The platform must support multiple AI providers through a provider-agnostic AI Gateway.

## Non-negotiable architecture
- Frontend never calls AI providers directly.
- All provider access goes through the backend AI Gateway.
- Models, capabilities and pricing are configuration/database driven.
- Provider API keys remain server-side.
- AI generations are asynchronous jobs.
- Billing must be idempotent and auditable.
- Never hard-code provider prices in UI or business logic.
- Never invent external API contracts. Verify current official provider documentation before implementing integrations.

## Product capabilities
Text-to-image, image-to-image/editing, image-to-video, text-to-video, video transformation, audio/TTS where supported, prompt enhancement, projects, assets, generation history, credits, subscriptions, pay-as-you-go, Stripe billing, usage metering, public API, API keys, webhooks, admin controls, moderation, rate limiting, retries and provider failover.

## Suggested stack
Use a modular monolith first:
- Next.js + TypeScript for web UI
- .NET 8 Web API for backend
- PostgreSQL
- Redis
- Background workers/queue
- S3-compatible object storage
- Stripe
- Docker Compose for development

Keep provider integrations behind interfaces so additional providers can be added later.

## Development workflow
1. Read this file and the relevant docs before coding.
2. Inspect the existing repository before changing it.
3. Make a short implementation plan.
4. Implement production-quality code; do not create fake integrations disguised as complete functionality.
5. Add/update tests.
6. Run tests, lint/type checks and builds.
7. Verify database migrations.
8. Update progress.md and decisions.md when appropriate.
9. Do not rewrite working architecture without a documented reason.

## Definition of done
A feature is complete only when implementation, validation, persistence, error handling, security, tests, documentation and verification are addressed.

## Documentation map
Read:
- docs/01-product.md
- docs/02-architecture.md
- docs/03-tech-stack.md
- docs/04-database.md
- docs/05-api.md
- docs/ai/*
- docs/billing/*
- docs/frontend/*
- docs/infrastructure/*
- docs/security/*
- docs/admin/*

## External APIs
For OpenAI, Google and future providers:
- use official documentation as the source of truth;
- verify authentication, endpoints, model identifiers, request/response formats, async behavior, webhooks/polling, limits and current pricing;
- isolate provider-specific behavior inside adapters;
- write mocked contract tests and integration tests where credentials are available.

## Security
Never expose provider keys, Stripe secrets or internal credentials to the browser.
Verify signed webhooks. Enforce authorization and rate limits. Store audit records for financial and administrative actions.

## Billing
Generation flow:
validate -> estimate price -> show estimate -> reserve credits -> queue -> execute -> record actual usage/cost -> capture actual credits -> refund/release difference -> finalize.

Failed/cancelled jobs must follow explicit refund rules.

## Project state
Maintain:
- progress.md
- decisions.md

Do not claim a feature is complete until it has been verified.
