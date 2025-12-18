using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("Nodes", t =>
        {
            t.HasCheckConstraint(
                "CK_Nodes_NodeType",
                "[NodeType] IN ('BACnetMSTP','BACnetIP','ModbusTCP','ModbusRTU','TCPIPGateway','EthernetInterface','SerialInterface','Other')"
            );
        });

        builder.HasKey(x => x.NodeId);

        builder.Property(x => x.NodeName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.NodeType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Protocol).HasMaxLength(50);
        builder.Property(x => x.NetworkAddress).HasMaxLength(50);
        builder.Property(x => x.BusAssociation).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.ControllerId).HasDatabaseName("idx_nodes_controller");
        builder.HasIndex(x => x.ProjectId).HasDatabaseName("idx_nodes_project");
        builder.HasIndex(x => x.Protocol).HasDatabaseName("idx_nodes_protocol");
        builder.HasIndex(x => new { x.ControllerId, x.NodeName }).IsUnique();

        builder.HasOne(x => x.Controller)
            .WithMany()
            .HasForeignKey(x => x.ControllerId)
            .OnDelete(DeleteBehavior.NoAction);

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


