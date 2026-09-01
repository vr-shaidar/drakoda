# Storage

Use S3-compatible object storage for source and generated media.

Separate:
originals, generated, thumbnails, temporary files.

Use signed URLs for private assets.
Set lifecycle/retention rules for temporary files.
Do not store large binary media in PostgreSQL.
