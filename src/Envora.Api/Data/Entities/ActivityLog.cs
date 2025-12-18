namespace Envora.Api.Data.Entities;

public sealed class ActivityLog
{
    public Guid ActivityId { get; set; }
    public Guid ProjectId { get; set; }

    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string? Action { get; set; }
    public Guid? UserId { get; set; }
    public string? Description { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime CreatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public User? User { get; set; }
}


