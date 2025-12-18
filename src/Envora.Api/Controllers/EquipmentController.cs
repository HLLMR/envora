using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/equipment")]
public sealed class EquipmentController(IEquipmentService equipment) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] Guid projectId, CancellationToken ct)
    {
        var result = await equipment.ListByProjectAsync(projectId, ct);
        return Ok(new { data = result });
    }

    [HttpGet("{equipmentId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, CancellationToken ct)
    {
        var item = await equipment.GetAsync(projectId, equipmentId, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromBody] CreateEquipmentRequest request, CancellationToken ct)
    {
        var created = await equipment.CreateAsync(projectId, request, ct);
        return CreatedAtAction(nameof(Get), new { projectId, equipmentId = created.EquipmentId }, created);
    }

    [HttpPatch("{equipmentId:guid}")]
    public async Task<IActionResult> Patch([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, [FromBody] UpdateEquipmentRequest request, CancellationToken ct)
    {
        var updated = await equipment.PatchAsync(projectId, equipmentId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{equipmentId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid equipmentId, CancellationToken ct)
    {
        var deleted = await equipment.DeleteAsync(projectId, equipmentId, ct);
        return deleted ? NoContent() : NotFound();
    }
}


