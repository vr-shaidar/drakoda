# OpenAI provider verification

Verified against current official OpenAI API model documentation on 2026-09-01.

## Current image generation

The official model catalog currently identifies `gpt-image-2` as the state-of-the-art image generation model. The adapter must therefore resolve image generation through the model registry rather than embedding provider model IDs in business logic.

## Architecture requirement

OpenAI access remains isolated behind `IAIProviderAdapter`. API credentials are server-side configuration only and must never be returned to the browser.

## Verification status

Model/catalog verification: COMPLETE against official documentation.
Credential-based integration test: REQUIRES EXTERNAL VERIFICATION.

Do not mark provider execution as production-verified until a real API credential is configured and an integration test succeeds.
