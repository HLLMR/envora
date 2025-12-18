using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class ValveScheduleItemConfiguration : IEntityTypeConfiguration<ValveScheduleItem>
{
    public void Configure(EntityTypeBuilder<ValveScheduleItem> builder)
    {
        builder.ToTable("ValveSchedule");
        builder.HasKey(x => x.ValveScheduleId);

        builder.Property(x => x.ValveTag).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Manufacturer).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.Size).HasMaxLength(50);
        builder.Property(x => x.Type).HasMaxLength(100);
        builder.Property(x => x.Pressure).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Temperature).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Material).HasMaxLength(100);
        builder.Property(x => x.ActuatorType).HasMaxLength(100);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.ProjectId, x.ValveTag }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


