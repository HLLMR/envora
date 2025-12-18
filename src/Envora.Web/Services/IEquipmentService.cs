using Envora.Web.Models.Equipment;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public interface IEquipmentService
{
    Task<ListResponse<EquipmentDto>> ListAsync(Guid projectId, CancellationToken ct);

    Task<EquipmentDto> CreateAsync(Guid projectId, CreateEquipmentRequest request, CancellationToken ct);
}


