using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Drakoda.Api.AI.Providers.Google;

public sealed class GoogleGeminiProviderAdapter(IHttpClientFactory httpClientFactory, IConfiguration configuration)
{
    public async Task<JsonDocument> GenerateImageAsync(string model, string prompt, string? aspectRatio, string? imageSize, CancellationToken ct)
    {
        var apiKey = configuration["Providers:Google:ApiKey"] ?? throw new InvalidOperationException("Google provider API key is not configured.");
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://generativelanguage.googleapis.com/v1beta/interactions");
        request.Headers.Add("x-goog-api-key", apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(new { model, input = new[] { new { type = "text", text = prompt } }, response_format = new { type = "image", aspect_ratio = aspectRatio, image_size = imageSize } }), Encoding.UTF8, "application/json");
        var response = await httpClientFactory.CreateClient().SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);
        response.EnsureSuccessStatusCode();
        return JsonDocument.Parse(body);
    }

    public async Task<JsonDocument> StartVideoAsync(string model, string prompt, string? aspectRatio, string? resolution, string? durationSeconds, CancellationToken ct)
    {
        var apiKey = configuration["Providers:Google:ApiKey"] ?? throw new InvalidOperationException("Google provider API key is not configured.");
        using var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/{Uri.EscapeDataString(model)}:predictLongRunning");
        request.Headers.Add("x-goog-api-key", apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(new { instances = new[] { new { prompt } }, parameters = new { aspectRatio, resolution, durationSeconds } }), Encoding.UTF8, "application/json");
        var response = await httpClientFactory.CreateClient().SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);
        response.EnsureSuccessStatusCode();
        return JsonDocument.Parse(body);
    }

    public async Task<JsonDocument> GetOperationAsync(string operationName, CancellationToken ct)
    {
        var apiKey = configuration["Providers:Google:ApiKey"] ?? throw new InvalidOperationException("Google provider API key is not configured.");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://generativelanguage.googleapis.com/v1beta/{operationName.TrimStart('/')}");
        request.Headers.Add("x-goog-api-key", apiKey);
        var response = await httpClientFactory.CreateClient().SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);
        response.EnsureSuccessStatusCode();
        return JsonDocument.Parse(body);
    }
}
