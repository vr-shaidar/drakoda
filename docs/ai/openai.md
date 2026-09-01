# OpenAI Integration

Implement an OpenAI adapter behind the AI Gateway.

Before implementation, verify the current official OpenAI API documentation for:
- current image generation/editing APIs and supported models
- current video generation APIs/models if enabled for the project
- authentication
- asynchronous job behavior
- input/output formats
- limits
- current pricing

Do not invent endpoint names or model identifiers.

Map provider errors into stable internal error categories.

Provider secrets remain server-side.
