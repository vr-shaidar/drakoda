# Google Veo 3.1 API Verification

Verified against Google's official Gemini API documentation on 2026-09-01.

## Current model identifiers

- `veo-3.1-generate-preview`
- `veo-3.1-fast-generate-preview`
- `veo-3.1-lite-generate-preview`

## Current capabilities

Veo 3.1 supports asynchronous video generation and returns a long-running operation that must be polled until completion.

Supported inputs/capabilities include:

- Text to video
- Image to video
- First frame to last frame interpolation
- Up to three reference images on supported variants
- Video extension on supported variants
- Native generated audio
- 16:9 and 9:16 aspect ratios
- 720p, 1080p and 4K on supported variants

Veo 3.1 video generation is currently documented as an 8-second generation operation. Video extension can extend eligible Veo-generated videos by 7 seconds per operation, subject to Google's documented limits.

## Adapter requirements

The provider adapter must:

1. Submit the request asynchronously.
2. Persist the returned provider operation name as `ExternalJobId`.
3. Poll the operation through the provider adapter.
4. Download completed media using authenticated provider access.
5. Return normalized output metadata to the generation engine.
6. Convert provider failures and safety blocks into normalized platform errors.

The application must never expose the Gemini API key to the browser.

## Verification status

API schema/capability verification: COMPLETE.

Live credential-based execution: REQUIRES EXTERNAL VERIFICATION.

Source: Google AI for Developers, Gemini API Veo 3.1 documentation, last updated 2026-08-30 UTC.
