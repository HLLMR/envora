using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;

namespace Envora.Api.Services.Interfaces;

public interface IPointService
{
    Task<IReadOnlyList<PointDto>> ListByEquipmentAsync(Guid projectId, Guid equipmentId, CancellationToken ct);

    Task<PointDto?> GetAsync(Guid projectId, Guid equipmentId, Guid pointId, CancellationToken ct);

    Task<PointDto> CreateAsync(Guid projectId, Guid equipmentId, CreatePointRequest request, CancellationToken ct);

    Task<PointDto?> PatchAsync(Guid projectId, Guid equipmentId, Guid pointId, UpdatePointRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, Guid pointId, CancellationToken ct);
}


