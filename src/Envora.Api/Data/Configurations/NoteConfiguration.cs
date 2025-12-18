using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Envora.Api.Data.Configurations;

public sealed class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes", t =>
        {
            t.HasCheckConstraint(
                "CK_Notes_Discipline",
                "[Discipline] IS NULL OR [Discipline] IN ('Overview','Financial','Schedule','Design','Service')"
            );
        });

        builder.HasKey(x => x.NoteId);

        builder.Property(x => x.Discipline).HasMaxLength(50);
        builder.Property(x => x.Content).IsRequired();

        builder.Property(x => x.IsResolved).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.ProjectId, x.Discipline })
            .HasDatabaseName("idx_notes_project_discipline");

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ParentNote)
            .WithMany()
            .HasForeignKey(x => x.ParentNoteId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ResolvedByUser)
            .WithMany()
            .HasForeignKey(x => x.ResolvedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


