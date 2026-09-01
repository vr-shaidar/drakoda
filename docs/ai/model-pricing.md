# Model Pricing

Keep provider cost and customer price separate.

Pricing dimensions may include:
- request
- image
- video second
- audio second
- token
- megapixel
- resolution
- quality
- output count

Pricing record:
model, pricingType, unit, resolution, quality, providerUnitCost, customerUnitPrice, currency, creditRate, minimumCharge, effectiveFrom, effectiveTo, active.

Never assume provider pricing is static. Provider-specific pricing must be verified from official documentation and stored as versioned configuration.
