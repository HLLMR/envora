using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects")]
public sealed class ProjectsController(IProjectService projects) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] string? status = null,
        [FromQuery] Guid? customerId = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken ct = default)
    {
        var result = await projects.ListAsync(skip, take, status, customerId, searchTerm, ct);
        return Ok(result);
    }

    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid projectId, CancellationToken ct)
    {
        var project = await projects.GetAsync(projectId, ct);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpGet("{projectId:guid}/detail")]
    public async Task<IActionResult> GetDetail([FromRoute] Guid projectId, CancellationToken ct)
    {
        var project = await projects.GetDetailAsync(projectId, ct);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken ct)
    {
        var created = await projects.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { projectId = created.ProjectId }, created);
    }

    [HttpPut("{projectId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid projectId, [FromBody] UpdateProjectRequest request, CancellationToken ct)
    {
        var updated = await projects.UpdateAsync(projectId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, CancellationToken ct)
    {
        var deleted = await projects.DeleteAsync(projectId, ct);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("dashboard/stats")]
    public async Task<IActionResult> GetDashboardStats(CancellationToken ct)
    {
        var stats = await projects.GetDashboardStatsAsync(ct);
        return Ok(stats);
    }
}


