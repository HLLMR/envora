using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/equipment/{equipmentId:guid}/points")]
public sealed class PointsController(IPointService points) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, CancellationToken ct)
    {
        var result = await points.ListByEquipmentAsync(projectId, equipmentId, ct);
        return Ok(new { data = result });
    }

    [HttpGet("{pointId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, [FromRoute] Guid pointId, CancellationToken ct)
    {
        var point = await points.GetAsync(projectId, equipmentId, pointId, ct);
        return point is null ? NotFound() : Ok(point);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, [FromBody] CreatePointRequest request, CancellationToken ct)
    {
        var created = await points.CreateAsync(projectId, equipmentId, request, ct);
        return CreatedAtAction(nameof(Get), new { projectId, equipmentId, pointId = created.PointId }, created);
    }

    [HttpPatch("{pointId:guid}")]
    public async Task<IActionResult> Patch([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, [FromRoute] Guid pointId, [FromBody] UpdatePointRequest request, CancellationToken ct)
    {
        var updated = await points.PatchAsync(projectId, equipmentId, pointId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{pointId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, [FromRoute] Guid pointId, CancellationToken ct)
    {
        var deleted = await points.DeleteAsync(projectId, equipmentId, pointId, ct);
        return deleted ? NoContent() : NotFound();
    }
}


