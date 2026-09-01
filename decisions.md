# Architecture Decisions

## ADR-001: Modular monolith first
Use a modular monolith instead of microservices initially. Extract services only when justified by measured scale or ownership requirements.

## ADR-002: Provider abstraction
All AI providers are accessed through provider adapters behind the AI Gateway.

## ADR-003: Database-driven models and pricing
Model catalog, capabilities and pricing are data-driven and versioned.

## ADR-004: Credit wallet
Internal credits are used to authorize and account for AI generation.

## ADR-005: Async generations
Long-running AI generation is always handled through durable background jobs.

## ADR-006: Development infrastructure
Use Docker Compose for local development with PostgreSQL, Redis and an S3-compatible MinIO service. The application services are containerized independently so the same boundaries can be promoted to staging/production infrastructure.

## ADR-007: Foundation API shape
Keep the initial .NET application as a modular monolith with dependency-injection boundaries. EF Core owns PostgreSQL persistence and StackExchange.Redis provides the Redis client; provider-specific SDKs will not be introduced until the corresponding official API is verified.

## ADR-008: Provider integration gate
No provider implementation is represented as production-ready until its current official authentication, model identifiers, endpoints, request/response formats, async behavior, limits and pricing have been verified.
