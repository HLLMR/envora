using System.Net.Http.Json;
using Envora.Web.Models.Projects;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class ProjectsService(HttpClient http) : IProjectsService
{
    public async Task<PaginatedResponse<ProjectListItemDto>> ListAsync(
        int skip,
        int take,
        string? status,
        string? searchTerm,
        CancellationToken ct)
    {
        var url = $"api/v1/projects?skip={skip}&take={take}";
        if (!string.IsNullOrWhiteSpace(status)) url += $"&status={Uri.EscapeDataString(status)}";
        if (!string.IsNullOrWhiteSpace(searchTerm)) url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";

        var result = await http.GetFromJsonAsync<PaginatedResponse<ProjectListItemDto>>(url, ct);
        return result ?? new PaginatedResponse<ProjectListItemDto>();
    }

    public async Task<ProjectListItemDto> CreateAsync(CreateProjectRequest request, CancellationToken ct)
    {
        var res = await http.PostAsJsonAsync("api/v1/projects", request, ct);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<ProjectListItemDto>(cancellationToken: ct))!;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct)
    {
        var result = await http.GetFromJsonAsync<DashboardStatsDto>("api/v1/projects/dashboard/stats", ct);
        return result ?? new DashboardStatsDto();
    }
}


