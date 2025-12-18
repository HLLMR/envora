namespace Envora.Api.Models.Dtos;

public sealed record DeviceDto(
    Guid DeviceId,
    Guid ProjectId,
    string DeviceName,
    string DeviceType,
    string? Category,
    Guid? MountedOnEquipmentId,
    string? Manufacturer,
    string? Model,
    bool IsActive
);


