using System.Net;
using System.Net.Http.Json;
using Envora.Web.Models.Equipment;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class EquipmentService(HttpClient http) : IEquipmentService
{
    private static async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
            return result ?? throw new ApiException("Invalid response from server", (int)response.StatusCode);
        }

        await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
        throw new InvalidOperationException("Unreachable");
    }

    public async Task<ListResponse<EquipmentDto>> ListAsync(Guid projectId, CancellationToken ct)
    {
        try
        {
            var response = await http.GetAsync($"api/v1/projects/{projectId}/equipment", ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ListResponse<EquipmentDto>>(cancellationToken: ct);
                return result ?? new ListResponse<EquipmentDto>();
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return new ListResponse<EquipmentDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<EquipmentDto> CreateAsync(Guid projectId, CreateEquipmentRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PostAsJsonAsync($"api/v1/projects/{projectId}/equipment", request, ct);
            return await HandleResponseAsync<EquipmentDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<EquipmentDto?> UpdateAsync(Guid projectId, Guid equipmentId, UpdateEquipmentRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PatchAsJsonAsync($"api/v1/projects/{projectId}/equipment/{equipmentId}", request, ct);
            if (res.StatusCode == HttpStatusCode.NotFound) return null;
            return await HandleResponseAsync<EquipmentDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, CancellationToken ct)
    {
        try
        {
            var res = await http.DeleteAsync($"api/v1/projects/{projectId}/equipment/{equipmentId}", ct);
            if (!res.IsSuccessStatusCode && res.StatusCode != HttpStatusCode.NotFound)
            {
                await ApiErrorHelper.ThrowApiExceptionAsync(res, ct);
            }
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }
}


