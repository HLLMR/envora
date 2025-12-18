using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Models.Shared;

namespace Envora.Api.Services.Interfaces;

public interface IProjectService
{
    Task<PaginatedResponse<ProjectListItemDto>> ListAsync(
        int skip,
        int take,
        string? status,
        Guid? customerId,
        string? searchTerm,
        CancellationToken ct);

    Task<ProjectListItemDto?> GetAsync(Guid projectId, CancellationToken ct);

    Task<ProjectDetailDto?> GetDetailAsync(Guid projectId, CancellationToken ct);

    Task<ProjectListItemDto> CreateAsync(CreateProjectRequest request, CancellationToken ct);

    Task<ProjectListItemDto?> UpdateAsync(Guid projectId, UpdateProjectRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid projectId, CancellationToken ct);

    Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct);
}


