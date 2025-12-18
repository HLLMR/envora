namespace Envora.Api.Data.Entities;

public sealed class Deliverable
{
    public Guid DeliverableId { get; set; }
    public Guid ProjectId { get; set; }

    public string DeliverableType { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Status { get; set; }

    public DateOnly? DueDate { get; set; }
    public DateOnly? SubmittedDate { get; set; }
    public DateOnly? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }

    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Project Project { get; set; } = null!;
    public User? ApprovedByUser { get; set; }
    public User? CreatedByUser { get; set; }
}


