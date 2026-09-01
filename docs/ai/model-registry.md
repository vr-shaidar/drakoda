# Model Registry

Models are database-driven.

Fields should include:
id, providerId, externalModelId, displayName, mediaType, enabled, capabilities, supportedAspectRatios, resolutions, durations, inputTypes, outputTypes, maxConcurrency, priority, metadata.

Never scatter model names across code.

Admin can enable/disable models and configure metadata without changing application code where possible.
