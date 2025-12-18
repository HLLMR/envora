namespace Envora.Api.Models.Dtos;

public sealed class NoteDto
{
    public Guid NoteId { get; set; }
    public Guid ProjectId { get; set; }
    public string? ContentContext { get; set; }
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public Guid? ParentNoteId { get; set; }
    public int Replies { get; set; }
    public Dictionary<string, int> Reactions { get; set; } = new();
}

