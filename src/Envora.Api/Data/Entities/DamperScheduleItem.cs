namespace Envora.Api.Data.Entities;

public sealed class DamperScheduleItem
{
    public Guid DamperScheduleId { get; set; }
    public Guid ProjectId { get; set; }

    public string DamperTag { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Size { get; set; }
    public string? Type { get; set; }
    public decimal? Pressure { get; set; }
    public string? Position { get; set; }
    public string? ActuatorType { get; set; }
    public int? Quantity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
}


