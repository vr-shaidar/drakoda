# Database Design

## Core entities
Users
Organizations
Memberships
Projects
Assets
Generations
GenerationJobs
GenerationInputs
GenerationOutputs

Providers
Models
ModelCapabilities
ModelPricing

Plans
Subscriptions
SubscriptionItems
CreditWallets
CreditTransactions
UsageRecords

Payments
Invoices
StripeEvents

ApiKeys
ApiUsage
Webhooks
WebhookEvents

AdminUsers
AuditLogs

## Key rules
- Monetary values use decimal/numeric, never floating point.
- Credits use integer/bigint units.
- Every financial transaction is immutable; corrections are compensating transactions.
- Generation IDs and provider job IDs are indexed.
- Provider/model IDs are unique.
- Pricing records are versioned with effective dates.
- Usage records preserve both provider cost and customer charge.
- Soft delete user-owned media where business rules require recovery.

## Generation status
QUEUED, PROCESSING, COMPLETED, FAILED, CANCELLED, REFUND_PENDING, REFUNDED.

## Credit transaction types
GRANT, PURCHASE, RESERVE, CAPTURE, RELEASE, REFUND, ADJUSTMENT, EXPIRATION.

## Required auditability
For every billable generation retain:
user, generation, model, provider, pricing version, estimated provider cost, actual provider cost when known, customer price, estimated credits, actual credits, status and timestamps.
