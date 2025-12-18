using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class ControllerConfiguration : IEntityTypeConfiguration<Controller>
{
    public void Configure(EntityTypeBuilder<Controller> builder)
    {
        builder.ToTable("Controllers", t =>
        {
            t.HasCheckConstraint(
                "CK_Controllers_ControllerType",
                "[ControllerType] IN ('VAVController','BMS','PLC','RTU','Thermostat','VFD','Gateway','Other')"
            );
        });

        builder.HasKey(x => x.ControllerId);

        builder.Property(x => x.ControllerName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ControllerType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Manufacturer).HasMaxLength(100);
        builder.Property(x => x.Model).HasMaxLength(100);
        builder.Property(x => x.FirmwareVersion).HasMaxLength(50);
        builder.Property(x => x.Location).HasMaxLength(255);

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_controllers_project");
        builder.HasIndex(x => new { x.ProjectId, x.ControllerName }).IsUnique();

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


