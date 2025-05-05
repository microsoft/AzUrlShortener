using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Cloud5mins.ShortenerTools.Core.Domain;
using System.Text.Json;

namespace Cloud5mins.ShortenerTools.Core.Service;

public class AzStorageBlobsService(BlobServiceClient client) : IAzStorageBlobsService
{
    private BlobContainerClient GetContainer(string containerName)
    {
        var container = client.GetBlobContainerClient(containerName);
        container.CreateIfNotExists();
        return container;
    }

    public async Task<string> GetBlobContentAsync(string blobName)
    {
        var container = GetContainer("urls");
        var blob = container.GetBlobClient(blobName);
        var response = await blob.DownloadAsync();
        using (var reader = new StreamReader(response.Value.Content))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public async Task<string> GetBlobContentAsync(string containerName, string blobName)
    {
        var container = GetContainer(containerName);
        var blob = container.GetBlobClient(blobName);
        var response = await blob.DownloadAsync();
        using (var reader = new StreamReader(response.Value.Content))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public async Task<string> GetBlobContentAsync(string containerName, string blobName, string contentType)
    {
        var container = GetContainer(containerName);
        var blob = container.GetBlobClient(blobName);
        var response = await blob.DownloadAsync();
        using (var reader = new StreamReader(response.Value.Content))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public async Task<string> UploadBlobAsync(string containerName, string blobName, Stream stream, string contentType)
    {
        var container = GetContainer(containerName);
        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });
        return blob.Uri.ToString();
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        var container = GetContainer(containerName);
        var blob = container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task<bool> BlobExistsAsync(string containerName, string blobName)
    {
        var container = GetContainer(containerName);
        var blob = container.GetBlobClient(blobName);
        return await blob.ExistsAsync();
    }

    public async Task<List<string>> ListBlobsAsync(string containerName)
    {
        var blobsList = new List<string>();
        var container = GetContainer(containerName);

        //await foreach (var item in container.GetBlobs())
        //    blobsList.Add(item.Name);

        return blobsList;
    }
}
