using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;

namespace Envora.Api.Services.Interfaces;

public interface IControllerService
{
    Task<IReadOnlyList<ControllerDto>> ListByProjectAsync(Guid projectId, CancellationToken ct);
    Task<ControllerDto?> GetAsync(Guid projectId, Guid controllerId, CancellationToken ct);
    Task<ControllerDto> CreateAsync(Guid projectId, CreateControllerRequest request, CancellationToken ct);
    Task<ControllerDto?> PatchAsync(Guid projectId, Guid controllerId, UpdateControllerRequest request, CancellationToken ct);
    Task<bool> DeleteAsync(Guid projectId, Guid controllerId, CancellationToken ct);
}


