namespace Envora.Api.Data.Entities;

public sealed class ControllerIoSlot
{
    public Guid IOSlotId { get; set; }
    public Guid ControllerId { get; set; }

    public string SlotName { get; set; } = null!;
    public string IOType { get; set; } = null!;
    public int? SlotNumber { get; set; }

    public string? DataType { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public string? Unit { get; set; }

    public bool? IsUsed { get; set; }
    public Guid? AssignedPointId { get; set; }

    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Controller Controller { get; set; } = null!;
    public Point? AssignedPoint { get; set; }
}


