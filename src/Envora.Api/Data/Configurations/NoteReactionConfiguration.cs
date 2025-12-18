using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class NoteReactionConfiguration : IEntityTypeConfiguration<NoteReaction>
{
    public void Configure(EntityTypeBuilder<NoteReaction> builder)
    {
        builder.ToTable("NoteReactions");
        builder.HasKey(x => x.ReactionId);

        builder.Property(x => x.Emoji).HasMaxLength(10).IsRequired();
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.NoteId, x.UserId, x.Emoji }).IsUnique();

        builder.HasOne(x => x.Note)
            .WithMany()
            .HasForeignKey(x => x.NoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


