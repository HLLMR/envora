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

    public async Task<PointDto?> UpdateAsync(Guid projectId, Guid equipmentId, Guid pointId, UpdatePointRequest request, CancellationToken ct)
    {
        var res = await http.PatchAsJsonAsync(
            $"api/v1/projects/{projectId}/equipment/{equipmentId}/points/{pointId}",
            request,
            ct);
        if (res.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<PointDto>(cancellationToken: ct);
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, Guid pointId, CancellationToken ct)
    {
        var res = await http.DeleteAsync(
            $"api/v1/projects/{projectId}/equipment/{equipmentId}/points/{pointId}",
            ct);
        return res.IsSuccessStatusCode;
    }
}


