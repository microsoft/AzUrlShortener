using Cloud5mins.ShortenerTools.Core.Domain;

namespace Cloud5mins.ShortenerTools.Core.Service;

public interface IAzStorageBlobsService
{
    Task<string> GetBlobContentAsync(string blobName);
    Task<string> GetBlobContentAsync(string containerName, string blobName);
    Task<string> GetBlobContentAsync(string containerName, string blobName, string contentType);
    Task<string> UploadBlobAsync(string containerName, string blobName, Stream stream, string contentType);
    Task DeleteBlobAsync(string containerName, string blobName);
    Task<bool> BlobExistsAsync(string containerName, string blobName);
    Task<List<string>> ListBlobsAsync(string containerName);
}

