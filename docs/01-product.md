# Product Requirements

## Goal
Create a multi-provider AI media SaaS where customers can generate, edit, organize and download AI media from one interface.

## Core modules
1. Authentication and accounts
2. Dashboard
3. AI Studio
4. Projects
5. Generations
6. Assets/media library
7. Credits and usage
8. Pricing and subscriptions
9. Payments
10. Public developer API
11. Admin
12. Moderation and abuse prevention

## AI Studio
Modes:
- Text to Image
- Image to Image / Edit
- Text to Video
- Image to Video
- Video Transformation
- Audio / Speech when supported

Common controls:
prompt, negative prompt where supported, model, aspect ratio, resolution, duration, quality, seed where supported, input assets, output count, advanced settings.

The UI must show an estimated credit cost before submission.

## Projects
Users can create projects containing prompts, generations, source assets, outputs, metadata and notes.

## Generation history
Each generation exposes status, model, provider, input, output, duration, resolution, estimated cost, actual cost, credits used, timestamps and errors when applicable.

## Plans
Support configurable plans such as Free, Starter, Creator, Pro and Business. Plan limits must be data-driven.

## Success criteria
A customer can register, purchase/receive credits, generate an image/video through a configured provider, see progress, retrieve the output, inspect usage and manage billing.
