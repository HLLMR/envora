namespace Envora.Api.Models.Dtos;

public sealed record ControllerIoSlotDto(
    Guid IoSlotId,
    Guid ControllerId,
    string SlotName,
    string IOType,
    int? SlotNumber,
    bool IsUsed,
    Guid? AssignedPointId
);


