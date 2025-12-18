namespace Envora.Api.Models.Dtos;

public sealed record PointDto(
    Guid PointId,
    Guid ProjectId,
    Guid EquipmentId,
    string PointTag,
    string SourceType,
    string PointType,
    string DataType,
    string? Unit,
    string? PointDescription
);


