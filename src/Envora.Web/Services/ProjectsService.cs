using System.Net;
using System.Net.Http.Json;
using Envora.Web.Models.Projects;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class ProjectsService(HttpClient http) : IProjectsService
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

    public async Task<PaginatedResponse<ProjectListItemDto>> ListAsync(
        int skip,
        int take,
        string? status,
        string? searchTerm,
        CancellationToken ct)
    {
        try
        {
            var url = $"api/v1/projects?skip={skip}&take={take}";
            if (!string.IsNullOrWhiteSpace(status)) url += $"&status={Uri.EscapeDataString(status)}";
            if (!string.IsNullOrWhiteSpace(searchTerm)) url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";

            var response = await http.GetAsync(url, ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<ProjectListItemDto>>(cancellationToken: ct);
                return result ?? new PaginatedResponse<ProjectListItemDto>();
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return new PaginatedResponse<ProjectListItemDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<ProjectListItemDto> CreateAsync(CreateProjectRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PostAsJsonAsync("api/v1/projects", request, ct);
            return await HandleResponseAsync<ProjectListItemDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct)
    {
        try
        {
            var response = await http.GetAsync("api/v1/projects/dashboard/stats", ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DashboardStatsDto>(cancellationToken: ct);
                return result ?? new DashboardStatsDto();
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return new DashboardStatsDto();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<ProjectDetailDto?> GetDetailAsync(Guid projectId, CancellationToken ct)
    {
        try
        {
            var response = await http.GetAsync($"api/v1/projects/{projectId}/detail", ct);
            if (response.StatusCode == HttpStatusCode.NotFound) 
            {
                // Try the basic endpoint as fallback
                var basicResponse = await http.GetAsync($"api/v1/projects/{projectId}", ct);
                if (basicResponse.IsSuccessStatusCode)
                {
                    var basicProject = await basicResponse.Content.ReadFromJsonAsync<ProjectListItemDto>(cancellationToken: ct);
                    if (basicProject != null)
                    {
                        // Convert to detail DTO
                        return new ProjectDetailDto
                        {
                            ProjectId = basicProject.ProjectId,
                            ProjectNumber = basicProject.ProjectNumber,
                            ProjectName = basicProject.ProjectName,
                            Status = basicProject.Status ?? "Unknown",
                            StartDate = basicProject.StartDate,
                            EstimatedCompletion = basicProject.EstimatedCompletion,
                            BudgetAmount = basicProject.BudgetAmount
                        };
                    }
                }
                return null;
            }
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProjectDetailDto>(cancellationToken: ct);
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return null;
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<ProjectListItemDto?> UpdateAsync(Guid projectId, UpdateProjectRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PutAsJsonAsync($"api/v1/projects/{projectId}", request, ct);
            if (res.StatusCode == HttpStatusCode.NotFound) return null;
            return await HandleResponseAsync<ProjectListItemDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }
}


