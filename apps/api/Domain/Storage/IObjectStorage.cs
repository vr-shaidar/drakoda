namespace Drakoda.Api.Domain.Storage;

public interface IObjectStorage
{
    Task<StoredObject> PutAsync(Stream content, string key, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default);
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);
    Task<string> CreateReadUrlAsync(string key, TimeSpan lifetime, CancellationToken cancellationToken = default);
}

public sealed record StoredObject(string Key, string ContentType, long? SizeBytes);
