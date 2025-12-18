using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", t =>
        {
            t.HasCheckConstraint(
                "CK_Users_Role",
                "[Role] IN ('Admin','ProjectManager','Estimator','DesignEngineer','Technician','ServiceCoordinator','ContractorSuper','Client')"
            );
        });
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Company).HasMaxLength(255);
        builder.Property(x => x.AzureAdId).HasMaxLength(255);
        builder.HasIndex(x => x.AzureAdId).IsUnique().HasFilter("[AzureAdId] IS NOT NULL");

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
    }
}


