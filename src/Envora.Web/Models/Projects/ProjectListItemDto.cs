namespace Envora.Web.Models.Projects;

public sealed class ProjectListItemDto
{
    public Guid ProjectId { get; set; }
    public string ProjectNumber { get; set; } = "";
    public string ProjectName { get; set; } = "";
    public string Status { get; set; } = "";
    public Guid? CustomerId { get; set; }
    public Guid? EngineeringFirmId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EstimatedCompletion { get; set; }
    public decimal? BudgetAmount { get; set; }
}


