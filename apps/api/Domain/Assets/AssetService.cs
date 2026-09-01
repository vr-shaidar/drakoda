namespace Drakoda.Api.Domain.Assets;

public interface IAssetService
{
    Task<AssetUpload> PrepareUploadAsync(Guid userId, string fileName, string contentType, long sizeBytes, CancellationToken cancellationToken);
    Task<string> CreateReadUrlAsync(Guid userId, Guid assetId, TimeSpan lifetime, CancellationToken cancellationToken);
}

public sealed record AssetUpload(Guid AssetId, string StorageKey, string UploadUrl);
