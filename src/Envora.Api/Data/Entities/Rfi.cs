namespace Envora.Api.Data.Entities;

public sealed class Rfi
{
    public Guid RfiId { get; set; }
    public Guid ProjectId { get; set; }

    public string RfiNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public Guid IssuedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }

    public string? Status { get; set; }
    public DateOnly? DueDate { get; set; }
    public DateOnly? ResponseDate { get; set; }
    public string? Response { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public User IssuedByUser { get; set; } = null!;
    public User? AssignedToUser { get; set; }
}


