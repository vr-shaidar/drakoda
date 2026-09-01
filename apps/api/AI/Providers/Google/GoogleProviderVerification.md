# Google provider verification

Verified against Google's official Gemini API documentation on 2026-09-01.

## Image

The Gemini API currently documents native image generation/editing with Gemini image models. Gemini 3.1 Flash Image supports image output and image sizes including 1K, 2K and 4K. The documented interaction endpoint is `/v1beta/interactions`.

## Video

Veo 3.1 supports text-to-video, image-to-video, video extension, first/last-frame interpolation, and up to three reference images. Documented output resolutions are 720p, 1080p and 4K depending on variant and duration. Video generation is asynchronous and returns a long-running operation that must be polled.

Current documented model IDs include:

- `veo-3.1-generate-preview`
- `veo-3.1-fast-generate-preview`
- `veo-3.1-lite-generate-preview`
- `gemini-3.1-flash-image`

Source: https://ai.google.dev/gemini-api/docs/veo
Source: https://ai.google.dev/gemini-api/docs/image-generation

## Implementation status

The provider adapter is implemented behind the application's provider boundary. Real credential-based integration testing remains **REQUIRES EXTERNAL VERIFICATION** until a Google API key is configured in the deployment environment.
