using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.ToTable("Points", t =>
        {
            t.HasCheckConstraint("CK_Points_SourceType", "[SourceType] IN ('SoftPoint','HardPoint')");
            t.HasCheckConstraint("CK_Points_PointType", "[PointType] IN ('Input','Output','Variable','Parameter')");
            t.HasCheckConstraint(
                "CK_Points_DataType",
                "[DataType] IN ('AnalogInput','AnalogOutput','DigitalInput','DigitalOutput','Integer','Real','String','Enumeration')"
            );
            t.HasCheckConstraint(
                "CK_Points_ControlPriority",
                "[ControlPriority] IS NULL OR [ControlPriority] IN ('Manual','Auto','Locked','Override')"
            );
        });

        builder.HasKey(x => x.PointId);

        builder.Property(x => x.PointTag).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PointDescription).HasMaxLength(255);

        builder.Property(x => x.SourceType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PointType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DataType).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Unit).HasMaxLength(50);
        builder.Property(x => x.MinValue).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MaxValue).HasColumnType("decimal(10,2)");
        builder.Property(x => x.DefaultValue).HasMaxLength(50);

        builder.Property(x => x.MinPhysical).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MaxPhysical).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MinRaw).HasColumnType("decimal(10,4)");
        builder.Property(x => x.MaxRaw).HasColumnType("decimal(10,4)");
        builder.Property(x => x.ScalingFactor).HasColumnType("decimal(10,4)");
        builder.Property(x => x.Offset).HasColumnType("decimal(10,4)");

        builder.Property(x => x.BACnetObjectName).HasMaxLength(100);

        builder.Property(x => x.Quality).HasMaxLength(50);
        builder.Property(x => x.ControlPriority).HasMaxLength(50);

        builder.Property(x => x.IsMonitored).HasDefaultValue(true);
        builder.Property(x => x.IsLogged).HasDefaultValue(true);
        builder.Property(x => x.IsMultiTermination).HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_points_project");
        builder.HasIndex(x => x.EquipmentId).HasDatabaseName("idx_points_equipment");
        builder.HasIndex(x => x.DeviceId).HasDatabaseName("idx_points_device");
        builder.HasIndex(x => x.ControllerId).HasDatabaseName("idx_points_controller");
        builder.HasIndex(x => x.SourceType).HasDatabaseName("idx_points_source");
        builder.HasIndex(x => x.IsMultiTermination).HasDatabaseName("idx_points_multiterm");
        builder.HasIndex(x => new { x.ProjectId, x.PointTag }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Equipment)
            .WithMany()
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Controller)
            .WithMany()
            .HasForeignKey(x => x.ControllerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ControllerIoSlot)
            .WithMany()
            .HasForeignKey(x => x.ControllerIOSlotId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


