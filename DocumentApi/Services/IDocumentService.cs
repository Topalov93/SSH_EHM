using DocumentApi.Models;

namespace DocumentApi.Services
{
    public interface IDocumentService
    {
        Task<Document> CreateAsync(IFormFile? file, string? metadata);
        Task<IEnumerable<Document>> ListAsync(int page, int pageSize);
        Task<Document?> GetAsync(Guid id);

        Task<Document?> UpdateMetadataAsync(Guid id, Document updated);
        Task<Document?> PatchMetadataAsync(Guid id, Dictionary<string, string> updates);

        Task<Stream?> DownloadAsync(Guid id);
        Task<Document?> UpdateFileAsync(Guid id, IFormFile file);

        Task<bool> DeleteAsync(Guid id);
    }
}
