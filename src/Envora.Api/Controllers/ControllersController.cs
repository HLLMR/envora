using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/controllers")]
public sealed class ControllersController(IControllerService controllers) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] Guid projectId, CancellationToken ct)
    {
        var result = await controllers.ListByProjectAsync(projectId, ct);
        return Ok(new { data = result });
    }

    [HttpGet("{controllerId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid projectId, [FromRoute] Guid controllerId, CancellationToken ct)
    {
        var item = await controllers.GetAsync(projectId, controllerId, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromBody] CreateControllerRequest request, CancellationToken ct)
    {
        var created = await controllers.CreateAsync(projectId, request, ct);
        return CreatedAtAction(nameof(Get), new { projectId, controllerId = created.ControllerId }, created);
    }

    [HttpPatch("{controllerId:guid}")]
    public async Task<IActionResult> Patch([FromRoute] Guid projectId, [FromRoute] Guid controllerId, [FromBody] UpdateControllerRequest request, CancellationToken ct)
    {
        var updated = await controllers.PatchAsync(projectId, controllerId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{controllerId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid controllerId, CancellationToken ct)
    {
        var deleted = await controllers.DeleteAsync(projectId, controllerId, ct);
        return deleted ? NoContent() : NotFound();
    }
}


