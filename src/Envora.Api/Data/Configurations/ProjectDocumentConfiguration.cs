using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class ProjectDocumentConfiguration : IEntityTypeConfiguration<ProjectDocument>
{
    public void Configure(EntityTypeBuilder<ProjectDocument> builder)
    {
        builder.ToTable("ProjectDocuments", t =>
        {
            t.HasCheckConstraint(
                "CK_ProjectDocuments_DocumentType",
                "[DocumentType] IS NULL OR [DocumentType] IN ('Submittal','Drawing','Specification','Manual','Report','Other')"
            );
        });

        builder.HasKey(x => x.DocumentId);

        builder.Property(x => x.DocumentType).HasMaxLength(50);
        builder.Property(x => x.DocumentName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.BlobStorageUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.FileType).HasMaxLength(50);
        builder.Property(x => x.Version).HasDefaultValue(1);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.UploadedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_documents_project");
        builder.HasIndex(x => new { x.ProjectId, x.DocumentName, x.Version }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.UploadedByUser)
            .WithMany()
            .HasForeignKey(x => x.UploadedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


