namespace Envora.Api.Data.Entities;

public sealed class ValveScheduleItem
{
    public Guid ValveScheduleId { get; set; }
    public Guid ProjectId { get; set; }

    public string ValveTag { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Size { get; set; }
    public string? Type { get; set; }
    public decimal? Pressure { get; set; }
    public decimal? Temperature { get; set; }
    public string? Material { get; set; }
    public string? ActuatorType { get; set; }
    public int? Quantity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
}


