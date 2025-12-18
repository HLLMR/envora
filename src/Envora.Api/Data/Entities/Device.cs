namespace Envora.Api.Data.Entities;

public sealed class Device
{
    public Guid DeviceId { get; set; }
    public Guid ProjectId { get; set; }

    public string DeviceName { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
    public string? Category { get; set; }

    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? PartNumber { get; set; }
    public string? SerialNumber { get; set; }

    public string? PhysicalProperties { get; set; }
    public string? DatasheetUrl { get; set; }
    public string? IomUrl { get; set; }
    public string? WiringDiagramUrl { get; set; }

    public Guid? MountedOnEquipmentId { get; set; }
    public string? LocationDescription { get; set; }
    public int? Quantity { get; set; }
    public DateOnly? InstallationDate { get; set; }

    public string? ProjectSpecificSpecs { get; set; }

    public DateOnly? CommissioningDate { get; set; }
    public string? CommissioningStatus { get; set; }

    public bool IsActive { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Project Project { get; set; } = null!;
    public Equipment? MountedOnEquipment { get; set; }
    public User? CreatedByUser { get; set; }
}


