using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;

namespace Envora.Api.Services.Interfaces;

public interface INodeService
{
    Task<IReadOnlyList<NodeDto>> ListAsync(Guid projectId, Guid controllerId, CancellationToken ct);
    Task<NodeDto?> GetAsync(Guid projectId, Guid controllerId, Guid nodeId, CancellationToken ct);
    Task<NodeDto> CreateAsync(Guid projectId, Guid controllerId, CreateNodeRequest request, CancellationToken ct);
    Task<NodeDto?> PatchAsync(Guid projectId, Guid controllerId, Guid nodeId, UpdateNodeRequest request, CancellationToken ct);
    Task<bool> DeleteAsync(Guid projectId, Guid controllerId, Guid nodeId, CancellationToken ct);
}


