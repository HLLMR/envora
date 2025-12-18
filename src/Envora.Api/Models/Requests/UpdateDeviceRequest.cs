using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class UpdateDeviceRequest
{
    [MaxLength(100)]
    public string? DeviceName { get; set; }

    [MaxLength(100)]
    public string? DeviceType { get; set; }

    [MaxLength(50)]
    public string? Category { get; set; }

    public Guid? MountedOnEquipmentId { get; set; }

    [MaxLength(100)]
    public string? Manufacturer { get; set; }

    [MaxLength(100)]
    public string? Model { get; set; }

    public bool? IsActive { get; set; }
}


