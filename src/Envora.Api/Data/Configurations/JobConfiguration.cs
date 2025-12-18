using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs", t =>
        {
            t.HasCheckConstraint(
                "CK_Jobs_JobType",
                "[JobType] IN ('GenerateSubmittal','VisioExport','PDFGeneration','EquipmentSchedule','BOMGeneration','Other')"
            );
            t.HasCheckConstraint(
                "CK_Jobs_Status",
                "[Status] IN ('Queued','Processing','Completed','Failed','Cancelled')"
            );
        });

        builder.HasKey(x => x.JobId);

        builder.Property(x => x.JobType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("Queued");

        builder.Property(x => x.RetryCount).HasDefaultValue(0);
        builder.Property(x => x.MaxRetries).HasDefaultValue(3);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.ProjectId, x.Status }).HasDatabaseName("idx_jobs_project_status");
        builder.HasIndex(x => x.JobType).HasDatabaseName("idx_jobs_type");

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.RequestedByUser)
            .WithMany()
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


