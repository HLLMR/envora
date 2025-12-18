using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class DeliverableConfiguration : IEntityTypeConfiguration<Deliverable>
{
    public void Configure(EntityTypeBuilder<Deliverable> builder)
    {
        builder.ToTable("Deliverables", t =>
        {
            t.HasCheckConstraint(
                "CK_Deliverables_DeliverableType",
                "[DeliverableType] IN ('Submittal','IOM','AsBuilt','CommissioningReport')"
            );
            t.HasCheckConstraint(
                "CK_Deliverables_Status",
                "[Status] IS NULL OR [Status] IN ('NotStarted','InProgress','Ready','Submitted','ApprovedWithChanges','Approved','Rejected','Archived')"
            );
        });

        builder.HasKey(x => x.DeliverableId);

        builder.Property(x => x.DeliverableType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_deliverables_project");

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ApprovedByUser)
            .WithMany()
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


