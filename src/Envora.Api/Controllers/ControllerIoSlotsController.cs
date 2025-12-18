using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/controllers/{controllerId:guid}/io-slots")]
public sealed class ControllerIoSlotsController(IControllerIoSlotService slots) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] Guid projectId, [FromRoute] Guid controllerId, CancellationToken ct)
    {
        var result = await slots.ListAsync(projectId, controllerId, ct);
        return Ok(new { data = result });
    }
}


