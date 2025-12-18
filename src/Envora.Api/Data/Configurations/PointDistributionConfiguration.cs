using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class PointDistributionConfiguration : IEntityTypeConfiguration<PointDistribution>
{
    public void Configure(EntityTypeBuilder<PointDistribution> builder)
    {
        builder.ToTable("PointDistribution");
        builder.HasKey(x => x.DistributionId);

        builder.Property(x => x.LocalPointName).HasMaxLength(100);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.SoftPointId).HasDatabaseName("idx_distribution_soft");
        builder.HasIndex(x => x.ConsumingControllerId).HasDatabaseName("idx_distribution_consumer");
        builder.HasIndex(x => new { x.SoftPointId, x.ConsumingControllerId }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SoftPoint)
            .WithMany()
            .HasForeignKey(x => x.SoftPointId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ConsumingController)
            .WithMany()
            .HasForeignKey(x => x.ConsumingControllerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Node)
            .WithMany()
            .HasForeignKey(x => x.NodeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


