namespace Envora.Api.Data.Entities;

public sealed class Project
{
    public Guid ProjectId { get; set; }

    public string ProjectNumber { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }

    public Guid? CustomerId { get; set; }
    public Guid? EngineeringFirmId { get; set; }

    public string Status { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EstimatedCompletion { get; set; }
    public DateOnly? ActualCompletion { get; set; }

    public decimal? BudgetAmount { get; set; }

    public Guid? ProjectManagerId { get; set; }
    public Guid? DesignEngineer1Id { get; set; }
    public Guid? DesignEngineer2Id { get; set; }

    public string? Location { get; set; }
    public string? BuildingType { get; set; }
    public decimal? SquareFootage { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Company? Customer { get; set; }
    public Company? EngineeringFirm { get; set; }

    public User? ProjectManager { get; set; }
    public User? DesignEngineer1 { get; set; }
    public User? DesignEngineer2 { get; set; }
    public User? CreatedByUser { get; set; }
}


