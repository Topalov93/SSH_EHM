using DocumentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Data
{
    public class DocumentsDbContext : DbContext
    {
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options) { }
        public DbSet<Document> Documents => Set<Document>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().Property(d => d.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            base.OnModelCreating(modelBuilder);
        }
    }
}
