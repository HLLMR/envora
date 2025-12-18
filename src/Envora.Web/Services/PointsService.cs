using System.Net.Http.Json;
using Envora.Web.Models.Points;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class PointsService(HttpClient http) : IPointsService
{
    public async Task<ListResponse<PointDto>> ListAsync(Guid projectId, Guid equipmentId, CancellationToken ct)
    {
        var res = await http.GetFromJsonAsync<ListResponse<PointDto>>(
            $"api/v1/projects/{projectId}/equipment/{equipmentId}/points",
            ct);
        return res ?? new ListResponse<PointDto>();
    }

    public async Task<PointDto> CreateAsync(Guid projectId, Guid equipmentId, CreatePointRequest request, CancellationToken ct)
    {
        var res = await http.PostAsJsonAsync(
            $"api/v1/projects/{projectId}/equipment/{equipmentId}/points",
            request,
            ct);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<PointDto>(cancellationToken: ct))!;
    }
}


