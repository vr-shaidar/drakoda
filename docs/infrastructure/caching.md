# Caching

Cache read-heavy, non-sensitive data such as model catalogs and public pricing with short configurable TTLs.

Never rely on cache for financial correctness.
Invalidate model/pricing caches after admin changes.
