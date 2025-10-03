using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentApi.Models
{
    public class Document : IFileProvider
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(260)]
        public string FileName { get; set; } = null!;
        [Required, MaxLength(100)]
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
        [Required, MaxLength(500)]
        public string BlobName { get; set; } = null!;
        public string? Metadata { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
