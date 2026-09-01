# Video Generation

Support text-to-video and image-to-video where selected models support them.

Inputs:
prompt, source image when applicable, model, aspect ratio, resolution, duration, quality and model-specific options.

Because video jobs are asynchronous, the API returns a generation ID and status rather than holding the HTTP request open.

Provider-specific restrictions must be reflected in model capabilities.
