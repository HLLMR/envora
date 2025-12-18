namespace Envora.Api.Data.Entities;

public sealed class Note
{
    public Guid NoteId { get; set; }
    public Guid ProjectId { get; set; }

    public string? Discipline { get; set; }
    public Guid? ParentNoteId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = null!;
    public string? MentionedUserIds { get; set; }
    public bool? IsResolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Project Project { get; set; } = null!;
    public Note? ParentNote { get; set; }
    public User Author { get; set; } = null!;
    public User? ResolvedByUser { get; set; }
}


