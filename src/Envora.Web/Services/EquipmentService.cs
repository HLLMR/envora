using System.Net.Http.Json;
using Envora.Web.Models.Equipment;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class EquipmentService(HttpClient http) : IEquipmentService
{
    public async Task<ListResponse<EquipmentDto>> ListAsync(Guid projectId, CancellationToken ct)
    {
        var res = await http.GetFromJsonAsync<ListResponse<EquipmentDto>>($"api/v1/projects/{projectId}/equipment", ct);
        return res ?? new ListResponse<EquipmentDto>();
    }

    public async Task<EquipmentDto> CreateAsync(Guid projectId, CreateEquipmentRequest request, CancellationToken ct)
    {
        var res = await http.PostAsJsonAsync($"api/v1/projects/{projectId}/equipment", request, ct);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<EquipmentDto>(cancellationToken: ct))!;
    }
}


