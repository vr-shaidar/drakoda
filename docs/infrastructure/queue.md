# Infrastructure Queue

Use a durable background queue backed by Redis or another appropriate queue system.

Workers must be horizontally scalable.
Jobs must be idempotent.
Use visibility/lease timeouts.
Record attempts and failures.
