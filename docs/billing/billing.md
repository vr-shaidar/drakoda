# Billing Architecture

Billing consists of:
- credit wallet
- subscriptions
- one-time credit purchases
- usage metering
- Stripe integration
- invoices/payments
- refunds
- audit trail

Internal credit accounting is the source of truth for generation authorization.
Stripe is the source of truth for payment/subscription events.
