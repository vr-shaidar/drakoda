using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Drakoda.AI;

public sealed class OpenAIProviderAdapter(IHttpClientFactory clients, IConfiguration configuration) : IAIProviderAdapter
{
    public string ProviderKey => "openai";

    public async Task<ProviderGenerationResult> GenerateAsync(ProviderGenerationRequest request, CancellationToken cancellationToken)
    {
        if (!request.Capability.Equals("text-to-image", StringComparison.OrdinalIgnoreCase))
            throw new NotSupportedException("The OpenAI adapter currently implements text-to-image only.");

        var key = configuration["AI:OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key is not configured.");
        using var http = clients.CreateClient("openai");
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

        var payload = new Dictionary<string, object?> { ["model"] = request.ExternalModelId, ["prompt"] = request.Prompt };
        foreach (var pair in request.Settings) payload[pair.Key] = pair.Value;
        using var response = await http.PostAsync("/v1/images/generations", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"), cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode) throw new ProviderException("OPENAI_REQUEST_FAILED", body);
        using var json = JsonDocument.Parse(body);
        var data = json.RootElement.GetProperty("data");
        var first = data[0];
        var url = first.TryGetProperty("url", out var u) ? u.GetString() : null;
        var b64 = first.TryGetProperty("b64_json", out var b) ? b.GetString() : null;
        return new ProviderGenerationResult(url, b64, null, null, null);
    }
}

public sealed class ProviderException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
