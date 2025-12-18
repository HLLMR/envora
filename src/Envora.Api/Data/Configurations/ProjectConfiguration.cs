using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects", t =>
        {
            t.HasCheckConstraint(
                "CK_Projects_Status",
                "[Status] IN ('Conceptual','Design','Bidding','Awarded','Procurement','Installation','Startup','Complete')"
            );
        });
        builder.HasKey(x => x.ProjectId);

        builder.Property(x => x.ProjectNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.ProjectNumber).IsUnique();

        builder.Property(x => x.ProjectName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Location).HasMaxLength(255);
        builder.Property(x => x.BuildingType).HasMaxLength(100);
        builder.Property(x => x.SquareFootage).HasColumnType("decimal(10,2)");
        builder.Property(x => x.BudgetAmount).HasColumnType("decimal(15,2)");

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.Status).HasDatabaseName("idx_projects_status");
        builder.HasIndex(x => x.ProjectManagerId).HasDatabaseName("idx_projects_pm");

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.EngineeringFirm)
            .WithMany()
            .HasForeignKey(x => x.EngineeringFirmId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ProjectManager)
            .WithMany()
            .HasForeignKey(x => x.ProjectManagerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.DesignEngineer1)
            .WithMany()
            .HasForeignKey(x => x.DesignEngineer1Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.DesignEngineer2)
            .WithMany()
            .HasForeignKey(x => x.DesignEngineer2Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


