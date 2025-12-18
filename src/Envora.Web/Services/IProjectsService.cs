using Envora.Web.Models.Projects;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public interface IProjectsService
{
    Task<PaginatedResponse<ProjectListItemDto>> ListAsync(int skip, int take, string? status, string? searchTerm, CancellationToken ct);

    Task<ProjectListItemDto> CreateAsync(CreateProjectRequest request, CancellationToken ct);
}


