# AI Gateway Specification

## Goal
Expose provider-neutral media generation capabilities.

## Interfaces
GenerateImage(request)
GenerateVideo(request)
ImageToVideo(request)
TransformVideo(request)
GenerateAudio(request)
EnhancePrompt(request)
GetJobStatus(providerJobId)
CancelJob(providerJobId)

## Request lifecycle
1. Authenticate user
2. Authorize capability
3. Validate input
4. Resolve model
5. Validate model capability
6. Calculate price
7. Reserve credits
8. Create generation/job records
9. Enqueue work
10. Execute provider adapter
11. Persist provider job ID
12. Poll or process webhook
13. Store outputs
14. Calculate actual usage
15. Capture/refund credits
16. Publish completion event

## Provider adapter contract
Adapters must translate generic requests to provider-specific requests and translate provider responses back into normalized internal objects.
