using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Drakoda.AI;

public sealed class OpenAIProviderAdapter(IHttpClientFactory clients, IConfiguration configuration) : IAIProviderAdapter
{
    public string ProviderId => "openai";

    public bool Supports(GenerationMode mode, string externalModelId) =>
        mode == GenerationMode.TextToImage && string.Equals(externalModelId, "gpt-image-2", StringComparison.OrdinalIgnoreCase);

    public async Task<ProviderSubmission> SubmitAsync(ProviderContext context, CancellationToken cancellationToken)
    {
        var key = configuration["AI:OpenAI:ApiKey"] ?? throw new InvalidOperationException("OPENAI_API_KEY_NOT_CONFIGURED");
        using var http = clients.CreateClient("openai");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/images/generations");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
        var payload = new Dictionary<string, object?> { ["model"] = context.ExternalModelId, ["prompt"] = context.Prompt };
        foreach (var pair in context.Settings) payload[pair.Key] = pair.Value;
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var response = await http.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode) throw new ProviderException("OPENAI_REQUEST_FAILED", body);
        using var json = JsonDocument.Parse(body);
        var data = json.RootElement.GetProperty("data");
        if (data.GetArrayLength() == 0) throw new ProviderException("OPENAI_EMPTY_RESPONSE", "No image output returned.");
        return new ProviderSubmission(context.GenerationId.ToString("N"), "completed", new Dictionary<string, string> { ["response"] = body });
    }

    public Task<ProviderResult> GetResultAsync(ProviderContext context, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("OPENAI_IMAGE_OPERATION_IS_SYNCHRONOUS");

    public Task CancelAsync(ProviderContext context, CancellationToken cancellationToken) => Task.CompletedTask;
}

public sealed class ProviderException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
