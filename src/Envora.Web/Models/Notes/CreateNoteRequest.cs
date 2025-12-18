namespace Envora.Web.Models.Notes;

public sealed class CreateNoteRequest
{
    public string Content { get; set; } = null!;
    public string? ContentContext { get; set; }
    public Guid? ParentNoteId { get; set; }
    public List<string>? Mentions { get; set; }
}

