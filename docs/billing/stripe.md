# Stripe Integration

Use Stripe for:
customer records
checkout
subscriptions
one-time credit purchases
payment status
invoices
customer portal where appropriate.

Verify webhook signatures.
Store every received event ID and process each event at most once.

Never trust browser-provided prices or plan amounts.
Create checkout sessions from server-side product/price configuration.

Before implementation, verify the current official Stripe documentation.
