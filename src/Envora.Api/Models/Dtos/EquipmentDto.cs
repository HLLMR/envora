namespace Envora.Api.Models.Dtos;

public sealed record EquipmentDto(
    Guid EquipmentId,
    Guid ProjectId,
    string EquipmentTag,
    string EquipmentType,
    string? Manufacturer,
    string? Model,
    string? Location,
    string? Description
);


