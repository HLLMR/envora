namespace Envora.Api.Models.Dtos;

public sealed record NodeDto(
    Guid NodeId,
    Guid ProjectId,
    Guid ControllerId,
    string NodeName,
    string NodeType,
    string? Protocol,
    string? NetworkAddress,
    bool IsActive
);


