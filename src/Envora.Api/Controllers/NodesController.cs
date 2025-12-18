using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/controllers/{controllerId:guid}/nodes")]
public sealed class NodesController(INodeService nodes) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] Guid projectId, [FromRoute] Guid controllerId, CancellationToken ct)
    {
        var result = await nodes.ListAsync(projectId, controllerId, ct);
        return Ok(new { data = result });
    }

    [HttpGet("{nodeId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid projectId, [FromRoute] Guid controllerId, [FromRoute] Guid nodeId, CancellationToken ct)
    {
        var item = await nodes.GetAsync(projectId, controllerId, nodeId, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromRoute] Guid controllerId, [FromBody] CreateNodeRequest request, CancellationToken ct)
    {
        var created = await nodes.CreateAsync(projectId, controllerId, request, ct);
        return CreatedAtAction(nameof(Get), new { projectId, controllerId, nodeId = created.NodeId }, created);
    }

    [HttpPatch("{nodeId:guid}")]
    public async Task<IActionResult> Patch([FromRoute] Guid projectId, [FromRoute] Guid controllerId, [FromRoute] Guid nodeId, [FromBody] UpdateNodeRequest request, CancellationToken ct)
    {
        var updated = await nodes.PatchAsync(projectId, controllerId, nodeId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{nodeId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid controllerId, [FromRoute] Guid nodeId, CancellationToken ct)
    {
        var deleted = await nodes.DeleteAsync(projectId, controllerId, nodeId, ct);
        return deleted ? NoContent() : NotFound();
    }
}


