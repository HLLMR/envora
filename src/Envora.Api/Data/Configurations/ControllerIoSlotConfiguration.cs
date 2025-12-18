using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class ControllerIoSlotConfiguration : IEntityTypeConfiguration<ControllerIoSlot>
{
    public void Configure(EntityTypeBuilder<ControllerIoSlot> builder)
    {
        builder.ToTable("ControllerIOSlots", t =>
        {
            t.HasCheckConstraint("CK_ControllerIOSlots_IOType", "[IOType] IN ('AI','AO','DI','DO')");
        });

        builder.HasKey(x => x.IOSlotId);

        builder.Property(x => x.SlotName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.IOType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DataType).HasMaxLength(50);
        builder.Property(x => x.MinValue).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MaxValue).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Unit).HasMaxLength(50);

        builder.Property(x => x.IsUsed).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ControllerId).HasDatabaseName("idx_ioslots_controller");
        builder.HasIndex(x => x.IsUsed).HasDatabaseName("idx_ioslots_used");
        builder.HasIndex(x => new { x.ControllerId, x.SlotName }).IsUnique();

        builder.HasOne(x => x.Controller)
            .WithMany()
            .HasForeignKey(x => x.ControllerId)
            .OnDelete(DeleteBehavior.NoAction);

        // DDL has a FK to Points(PointId) with no cascade specified.
        builder.HasOne(x => x.AssignedPoint)
            .WithMany()
            .HasForeignKey(x => x.AssignedPointId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


