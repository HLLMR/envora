using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/devices")]
public sealed class DevicesController(IDeviceService devices) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] Guid projectId, CancellationToken ct)
    {
        var result = await devices.ListByProjectAsync(projectId, ct);
        return Ok(new { data = result });
    }

    [HttpGet("{deviceId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid projectId, [FromRoute] Guid deviceId, CancellationToken ct)
    {
        var item = await devices.GetAsync(projectId, deviceId, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid projectId, [FromBody] CreateDeviceRequest request, CancellationToken ct)
    {
        var created = await devices.CreateAsync(projectId, request, ct);
        return CreatedAtAction(nameof(Get), new { projectId, deviceId = created.DeviceId }, created);
    }

    [HttpPatch("{deviceId:guid}")]
    public async Task<IActionResult> Patch([FromRoute] Guid projectId, [FromRoute] Guid deviceId, [FromBody] UpdateDeviceRequest request, CancellationToken ct)
    {
        var updated = await devices.PatchAsync(projectId, deviceId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{deviceId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid projectId, [FromRoute] Guid deviceId, CancellationToken ct)
    {
        var deleted = await devices.DeleteAsync(projectId, deviceId, ct);
        return deleted ? NoContent() : NotFound();
    }
}


