using Drakoda.Api.Domain.Assets;

namespace Drakoda.Api.Domain.Storage;

public sealed class AssetService(DrakodaDbContext db, IObjectStorage storage)
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    { "image/jpeg", "image/png", "image/webp", "image/gif", "video/mp4", "video/webm", "audio/mpeg", "audio/wav", "audio/mp4" };

    public async Task<Asset> UploadAsync(Guid userId, Stream content, string fileName, string contentType, long sizeBytes, Guid? projectId, CancellationToken ct)
    {
        if (!Allowed.Contains(contentType)) throw new InvalidOperationException("Unsupported media type.");
        if (sizeBytes <= 0 || sizeBytes > 500L * 1024 * 1024) throw new InvalidOperationException("File size exceeds the allowed limit.");
        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || fileName.Contains("..", StringComparison.Ordinal)) throw new InvalidOperationException("Invalid file name.");
        if (projectId.HasValue && !await db.Projects.AnyAsync(x => x.Id == projectId && x.UserId == userId, ct)) throw new InvalidOperationException("Project not found.");

        var id = Guid.NewGuid();
        var key = $"users/{userId:D}/assets/{id:D}";
        await storage.PutAsync(content, key, contentType, ct);
        var asset = new Asset { Id = id, UserId = userId, ProjectId = projectId, FileName = Path.GetFileName(fileName), StorageKey = key, ContentType = contentType, SizeBytes = sizeBytes, CreatedAt = DateTimeOffset.UtcNow };
        db.Assets.Add(asset);
        await db.SaveChangesAsync(ct);
        return asset;
    }
}
