using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices", t =>
        {
            t.HasCheckConstraint(
                "CK_Devices_DeviceType",
                "[DeviceType] IN ('Relay','Enclosure','Terminal','Transformer','Wiring','Thermistor','RTD','Sensor','Actuator','Transducer','Valve','Damper','FlowMeter','UtilityMeter','AirflowStation','Other')"
            );
            t.HasCheckConstraint(
                "CK_Devices_CommissioningStatus",
                "[CommissioningStatus] IS NULL OR [CommissioningStatus] IN ('NotStarted','InProgress','Commissioned','Verified','Failed')"
            );
        });

        builder.HasKey(x => x.DeviceId);

        builder.Property(x => x.DeviceName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DeviceType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(50);

        builder.Property(x => x.Manufacturer).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.PartNumber).HasMaxLength(100);
        builder.Property(x => x.SerialNumber).HasMaxLength(100);

        builder.Property(x => x.DatasheetUrl).HasMaxLength(500);
        builder.Property(x => x.IomUrl).HasMaxLength(500).HasColumnName("IOMUrl");
        builder.Property(x => x.WiringDiagramUrl).HasMaxLength(500);

        builder.Property(x => x.LocationDescription).HasMaxLength(255);

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_devices_project");
        builder.HasIndex(x => x.MountedOnEquipmentId).HasDatabaseName("idx_devices_equipment");
        builder.HasIndex(x => x.DeviceType).HasDatabaseName("idx_devices_type");
        builder.HasIndex(x => new { x.ProjectId, x.DeviceName }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.MountedOnEquipment)
            .WithMany()
            .HasForeignKey(x => x.MountedOnEquipmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


