using Azure.Storage.Blobs;

namespace DocumentApi.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var conn = configuration.GetValue<string>("AzureBlob:ConnectionString") 
                ?? throw new InvalidOperationException("AzureBlob:ConnectionString missing");
            var container = configuration.GetValue<string>("AzureBlob:Container") ?? "documents";
            var blobServiceClient = new BlobServiceClient(conn);
            _containerClient = blobServiceClient.GetBlobContainerClient(container);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadStreamAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var blobName = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{Guid.NewGuid():N}";
            var blobClient = _containerClient.GetBlobClient(blobName);
            var headers = new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = contentType };
            await blobClient.UploadAsync(stream, headers, cancellationToken: cancellationToken);
            return blobName;
        }

        public BlobClient GetBlobClient(string blobName) => _containerClient.GetBlobClient(blobName);

        public async Task<bool> DeleteIfExistsAsync(string blobName)
        {
            var client = _containerClient.GetBlobClient(blobName);
            var resp = await client.DeleteIfExistsAsync();
            return resp.Value;
        }
    }
}
