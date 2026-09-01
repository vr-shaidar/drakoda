# Internal and Application API

## Authentication
POST /api/v1/auth/register
POST /api/v1/auth/login
POST /api/v1/auth/refresh
POST /api/v1/auth/logout

## Models
GET /api/v1/models
GET /api/v1/models/{id}

## Generation
POST /api/v1/generations
GET /api/v1/generations
GET /api/v1/generations/{id}
POST /api/v1/generations/{id}/cancel

## Assets
GET /api/v1/assets
GET /api/v1/assets/{id}
DELETE /api/v1/assets/{id}

## Projects
GET /api/v1/projects
POST /api/v1/projects
GET /api/v1/projects/{id}
PATCH /api/v1/projects/{id}
DELETE /api/v1/projects/{id}

## Billing
GET /api/v1/billing
GET /api/v1/billing/usage
GET /api/v1/billing/credits
POST /api/v1/billing/checkout

## Developer API
POST /v1/generations
GET /v1/generations/{id}
POST /v1/generations/{id}/cancel
GET /v1/models
GET /v1/usage

Use request id and idempotency-key headers. Return stable error codes, not provider-specific raw errors.
