namespace Envora.Api.Data.Entities;

public sealed class Controller
{
    public Guid ControllerId { get; set; }
    public Guid ProjectId { get; set; }

    public string ControllerName { get; set; } = null!;
    public string ControllerType { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }

    public int? AnalogInputCount { get; set; }
    public int? AnalogOutputCount { get; set; }
    public int? DigitalInputCount { get; set; }
    public int? DigitalOutputCount { get; set; }

    public DateOnly? CommissioningDate { get; set; }
    public DateOnly? WarrantyExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Project Project { get; set; } = null!;
    public User? CreatedByUser { get; set; }
}


