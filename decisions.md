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
