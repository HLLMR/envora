using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class CreateProjectRequest
{
    [Required]
    [MaxLength(50)]
    public string ProjectNumber { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? CustomerId { get; set; }
    public Guid? EngineeringFirmId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Conceptual";

    public DateOnly? StartDate { get; set; }
    public DateOnly? EstimatedCompletion { get; set; }
    public decimal? BudgetAmount { get; set; }

    public Guid? ProjectManagerId { get; set; }
    public Guid? DesignEngineer1Id { get; set; }
    public Guid? DesignEngineer2Id { get; set; }
}


