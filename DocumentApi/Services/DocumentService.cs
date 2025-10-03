using DocumentApi.Data;
using DocumentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentsDbContext _db;
        private readonly BlobStorageService _blob;

        public DocumentService(DocumentsDbContext db, BlobStorageService blob)
        {
            _db = db;
            _blob = blob;
        }

        public async Task<Document> CreateAsync(IFormFile? file, string? metadata)
        {
            string blobName = "";
            string? contentType = null;
            long size = 0;

            if (file != null)
            {
                using var stream = file.OpenReadStream();
                blobName = await _blob.UploadStreamAsync(stream, file.ContentType);
                contentType = file.ContentType;
                size = file.Length;
            }

            var doc = new Document
            {
                FileName = file?.FileName ?? "no-file",
                ContentType = contentType ?? "application/octet-stream",
                Size = size,
                BlobName = blobName,
                Metadata = metadata,
                CreatedAt = DateTime.UtcNow
            };

            _db.Documents.Add(doc);
            await _db.SaveChangesAsync();
            return doc;
        }

        public async Task<IEnumerable<Document>> ListAsync(int page, int pageSize)
        {
            return await _db.Documents
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Document?> GetAsync(Guid id)
        {
            return await _db.Documents.FindAsync(id);
        }

        public async Task<Document?> UpdateMetadataAsync(Guid id, Document updated)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return null;

            doc.Metadata = updated.Metadata;
            doc.FileName = updated.FileName ?? doc.FileName;
            doc.ContentType = updated.ContentType ?? doc.ContentType;
            doc.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return doc;
        }

        public async Task<Document?> PatchMetadataAsync(Guid id, Dictionary<string, string> updates)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return null;

            if (updates.ContainsKey("metadata"))
                doc.Metadata = updates["metadata"];

            if (updates.ContainsKey("fileName"))
                doc.FileName = updates["fileName"];

            doc.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return doc;
        }

        public async Task<Stream?> DownloadAsync(Guid id)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return null;

            var blob = _blob.GetBlobClient(doc.BlobName);
            if (!await blob.ExistsAsync()) return null;

            return await blob.OpenReadAsync();
        }

        public async Task<Document?> UpdateFileAsync(Guid id, IFormFile file)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return null;

            using var stream = file.OpenReadStream();
            var newBlobName = await _blob.UploadStreamAsync(stream, file.ContentType);

            _ = _blob.DeleteIfExistsAsync(doc.BlobName);

            doc.FileName = file.FileName;
            doc.ContentType = file.ContentType;
            doc.Size = file.Length;
            doc.BlobName = newBlobName;
            doc.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return doc;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return false;

            _db.Documents.Remove(doc);
            await _db.SaveChangesAsync();

            _ = _blob.DeleteIfExistsAsync(doc.BlobName);
            return true;
        }
    }
}
