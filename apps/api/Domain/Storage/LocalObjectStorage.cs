namespace Drakoda.Api.Domain.Storage;

public sealed class LocalObjectStorage(IConfiguration configuration) : IObjectStorage
{
    private readonly string _root = configuration["Storage:LocalPath"] ?? Path.Combine(AppContext.BaseDirectory, "storage");
    public async Task<StoredObject> PutAsync(Stream content, string key, string contentType, CancellationToken cancellationToken = default)
    {
        var path = Resolve(key); Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var output = File.Create(path); await content.CopyToAsync(output, cancellationToken);
        return new StoredObject(key, contentType, output.Length);
    }
    public Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default) => Task.FromResult<Stream>(File.OpenRead(Resolve(key)));
    public Task DeleteAsync(string key, CancellationToken cancellationToken = default) { var p = Resolve(key); if (File.Exists(p)) File.Delete(p); return Task.CompletedTask; }
    public Task<string> CreateReadUrlAsync(string key, TimeSpan lifetime, CancellationToken cancellationToken = default) => Task.FromResult($"/v1/assets/download/{Uri.EscapeDataString(key)}");
    private string Resolve(string key) { var clean = key.Replace('\\', '/').TrimStart('/'); var root = Path.GetFullPath(_root); var path = Path.GetFullPath(Path.Combine(root, clean)); if (!path.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Invalid storage key."); return path; }
}
