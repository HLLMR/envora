namespace Envora.Api.Data.Entities;

public sealed class Point
{
    public Guid PointId { get; set; }
    public Guid ProjectId { get; set; }

    public string PointTag { get; set; } = null!;
    public string? PointDescription { get; set; }

    public string SourceType { get; set; } = null!;

    public Guid? EquipmentId { get; set; }
    public Guid? DeviceId { get; set; }
    public Guid? ControllerId { get; set; }
    public Guid? ControllerIOSlotId { get; set; }

    public string PointType { get; set; } = null!;
    public string DataType { get; set; } = null!;

    public string? Unit { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public string? DefaultValue { get; set; }

    public decimal? MinPhysical { get; set; }
    public decimal? MaxPhysical { get; set; }
    public decimal? MinRaw { get; set; }
    public decimal? MaxRaw { get; set; }
    public decimal? ScalingFactor { get; set; }
    public decimal? Offset { get; set; }

    public string? BACnetObjectName { get; set; }
    public int? BACnetObjectInstance { get; set; }

    public bool? IsMonitored { get; set; }
    public bool? IsLogged { get; set; }
    public string? Quality { get; set; }
    public string? ControlPriority { get; set; }

    public bool? IsMultiTermination { get; set; }
    public Guid? ParentDeviceId { get; set; }

    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Project Project { get; set; } = null!;
    public Equipment? Equipment { get; set; }
    public Device? Device { get; set; }
    public Controller? Controller { get; set; }
    public ControllerIoSlot? ControllerIoSlot { get; set; }
    public User? CreatedByUser { get; set; }
}


