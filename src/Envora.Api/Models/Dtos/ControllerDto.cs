namespace Envora.Api.Models.Dtos;

public sealed record ControllerDto(
    Guid ControllerId,
    Guid ProjectId,
    string ControllerName,
    string ControllerType,
    string? Manufacturer,
    string? Model,
    bool IsActive
);


