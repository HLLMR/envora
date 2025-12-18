namespace Envora.Api.Models.Dtos;

public sealed class DashboardStatsDto
{
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int CompletedProjects { get; set; }
    public int OnHoldProjects { get; set; }
    public Dictionary<string, int> ProjectsByStatus { get; set; } = new();
    public int TotalEquipment { get; set; }
    public int TotalPoints { get; set; }
}

