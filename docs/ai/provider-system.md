# Provider System

Implement IAIProviderAdapter and capability-specific methods.

Provider-specific code must live under provider modules, e.g.:
OpenAIAdapter
GoogleAdapter

Each adapter must expose:
- provider identity
- supported models
- capabilities
- request validation
- submission
- status retrieval
- cancellation when supported
- error mapping
- cost/usage mapping

The router must choose an enabled model and adapter without frontend knowledge of provider implementation details.
