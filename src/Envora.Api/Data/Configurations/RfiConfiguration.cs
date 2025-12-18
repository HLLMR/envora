using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class RfiConfiguration : IEntityTypeConfiguration<Rfi>
{
    public void Configure(EntityTypeBuilder<Rfi> builder)
    {
        builder.ToTable("RFIs", t =>
        {
            t.HasCheckConstraint(
                "CK_RFIs_Status",
                "[Status] IS NULL OR [Status] IN ('Open','Acknowledged','InProgress','Responded','Closed')"
            );
        });

        builder.HasKey(x => x.RfiId);
        builder.Property(x => x.RfiId).HasColumnName("RFIId");

        builder.Property(x => x.RfiNumber).HasMaxLength(50).IsRequired().HasColumnName("RFINumber");
        builder.HasIndex(x => x.RfiNumber).IsUnique();

        builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.IssuedByUser)
            .WithMany()
            .HasForeignKey(x => x.IssuedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.AssignedToUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


