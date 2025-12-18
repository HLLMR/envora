using Envora.Web.Models.Points;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public interface IPointsService
{
    Task<ListResponse<PointDto>> ListAsync(Guid projectId, Guid equipmentId, CancellationToken ct);

    Task<PointDto> CreateAsync(Guid projectId, Guid equipmentId, CreatePointRequest request, CancellationToken ct);
}


