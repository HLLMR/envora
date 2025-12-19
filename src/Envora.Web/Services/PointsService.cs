using System.Net;
using System.Net.Http.Json;
using Envora.Web.Models.Points;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class PointsService(HttpClient http) : IPointsService
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

    public async Task<ListResponse<PointDto>> ListAsync(Guid projectId, Guid equipmentId, CancellationToken ct)
    {
        try
        {
            var response = await http.GetAsync(
                $"api/v1/projects/{projectId}/equipment/{equipmentId}/points",
                ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ListResponse<PointDto>>(cancellationToken: ct);
                return result ?? new ListResponse<PointDto>();
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return new ListResponse<PointDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<PointDto> CreateAsync(Guid projectId, Guid equipmentId, CreatePointRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PostAsJsonAsync(
                $"api/v1/projects/{projectId}/equipment/{equipmentId}/points",
                request,
                ct);
            return await HandleResponseAsync<PointDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<PointDto?> UpdateAsync(Guid projectId, Guid equipmentId, Guid pointId, UpdatePointRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PatchAsJsonAsync(
                $"api/v1/projects/{projectId}/equipment/{equipmentId}/points/{pointId}",
                request,
                ct);
            if (res.StatusCode == HttpStatusCode.NotFound) return null;
            return await HandleResponseAsync<PointDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, Guid pointId, CancellationToken ct)
    {
        try
        {
            var res = await http.DeleteAsync(
                $"api/v1/projects/{projectId}/equipment/{equipmentId}/points/{pointId}",
                ct);
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


