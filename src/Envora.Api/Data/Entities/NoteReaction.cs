namespace Envora.Api.Data.Entities;

public sealed class NoteReaction
{
    public Guid ReactionId { get; set; }
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
    public string Emoji { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Note Note { get; set; } = null!;
    public User User { get; set; } = null!;
}


