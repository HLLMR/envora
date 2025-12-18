using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class DamperScheduleItemConfiguration : IEntityTypeConfiguration<DamperScheduleItem>
{
    public void Configure(EntityTypeBuilder<DamperScheduleItem> builder)
    {
        builder.ToTable("DamperSchedule");
        builder.HasKey(x => x.DamperScheduleId);

        builder.Property(x => x.DamperTag).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Manufacturer).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.Size).HasMaxLength(50);
        builder.Property(x => x.Type).HasMaxLength(100);
        builder.Property(x => x.Pressure).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Position).HasMaxLength(50);
        builder.Property(x => x.ActuatorType).HasMaxLength(100);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.ProjectId, x.DamperTag }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


