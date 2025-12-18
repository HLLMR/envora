using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;

namespace Envora.Api.Services.Interfaces;

public interface IEquipmentService
{
    Task<IReadOnlyList<EquipmentDto>> ListByProjectAsync(Guid projectId, CancellationToken ct);

    Task<EquipmentDto?> GetAsync(Guid projectId, Guid equipmentId, CancellationToken ct);

    Task<EquipmentDto> CreateAsync(Guid projectId, CreateEquipmentRequest request, CancellationToken ct);

    Task<EquipmentDto?> PatchAsync(Guid projectId, Guid equipmentId, UpdateEquipmentRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, CancellationToken ct);
}


