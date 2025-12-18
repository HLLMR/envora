using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipment", t =>
        {
            t.HasCheckConstraint(
                "CK_Equipment_EquipmentType",
                "[EquipmentType] IN ('RTU','AHU','VAV','Pump','Fan','Chiller','Boiler','Damper','Valve','Sensor','Other')"
            );
        });

        builder.HasKey(x => x.EquipmentId);

        builder.Property(x => x.EquipmentTag).HasMaxLength(50).IsRequired();
        builder.Property(x => x.EquipmentType).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Manufacturer).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.Capacity).HasMaxLength(100);
        builder.Property(x => x.CapacityUnit).HasMaxLength(50);
        builder.Property(x => x.SerialNumber).HasMaxLength(100);
        builder.Property(x => x.Location).HasMaxLength(255);
        builder.Property(x => x.SpecSheetUrl).HasMaxLength(500);
        builder.Property(x => x.MaintenanceFrequency).HasMaxLength(50);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_equipment_project");
        builder.HasIndex(x => x.EquipmentType).HasDatabaseName("idx_equipment_type");
        builder.HasIndex(x => new { x.ProjectId, x.EquipmentTag }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


