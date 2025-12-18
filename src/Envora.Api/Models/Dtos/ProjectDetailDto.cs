namespace Envora.Api.Models.Dtos;

public sealed class ProjectDetailDto
{
    public Guid ProjectId { get; set; }
    public string ProjectNumber { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public Guid? EngineeringFirmId { get; set; }
    public string? EngineeringFirmName { get; set; }
    
    public DateOnly? StartDate { get; set; }
    public DateOnly? EstimatedCompletion { get; set; }
    public DateOnly? ActualCompletion { get; set; }
    public decimal? BudgetAmount { get; set; }
    
    public Guid? ProjectManagerId { get; set; }
    public string? ProjectManagerName { get; set; }
    public string? ProjectManagerEmail { get; set; }
    
    public Guid? DesignEngineer1Id { get; set; }
    public string? DesignEngineer1Name { get; set; }
    public string? DesignEngineer1Email { get; set; }
    
    public Guid? DesignEngineer2Id { get; set; }
    public string? DesignEngineer2Name { get; set; }
    public string? DesignEngineer2Email { get; set; }
    
    public string? Location { get; set; }
    public string? BuildingType { get; set; }
    public decimal? SquareFootage { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

