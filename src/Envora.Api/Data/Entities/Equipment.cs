namespace Envora.Api.Data.Entities;

public sealed class Equipment
{
    public Guid EquipmentId { get; set; }
    public Guid ProjectId { get; set; }

    public string EquipmentTag { get; set; } = null!;
    public string EquipmentType { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Capacity { get; set; }
    public string? CapacityUnit { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string? SpecSheetUrl { get; set; }
    public DateOnly? InstallationDate { get; set; }
    public DateOnly? WarrantyExpirationDate { get; set; }
    public string? MaintenanceFrequency { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Project Project { get; set; } = null!;
    public User? CreatedByUser { get; set; }
}


