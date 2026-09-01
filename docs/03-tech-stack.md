# Technology Stack

## Web
Next.js, TypeScript, React, server/client components as appropriate, responsive UI, accessible components, strong form validation.

## Backend
.NET 8 Web API, dependency injection, options/configuration pattern, background workers, structured logging, OpenAPI.

## Data
PostgreSQL with migrations and transactional consistency.
Redis for cache, distributed coordination, rate limiting and queue support where selected.

## Storage
S3-compatible object storage. Store metadata in PostgreSQL and media in object storage.

## Billing
Stripe for customer, checkout/subscription/payment and webhook flows.

## Infrastructure
Docker Compose for local development. Production deployment must support HTTPS, secrets management, backups, monitoring and horizontal worker scaling.

## Quality
Unit tests, integration tests, API contract tests, frontend tests and end-to-end tests for critical user journeys.
