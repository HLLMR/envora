using Envora.Api.Data;
using Envora.Api.Models.Dtos;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class ControllerIoSlotService(EnvoraDbContext db) : IControllerIoSlotService
{
    public async Task<IReadOnlyList<ControllerIoSlotDto>> ListAsync(Guid projectId, Guid controllerId, CancellationToken ct)
    {
        var controllerExists = await db.Controllers.AsNoTracking()
            .AnyAsync(c => c.ProjectId == projectId && c.ControllerId == controllerId, ct);
        if (!controllerExists)
        {
            throw new InvalidOperationException("Controller not found for project.");
        }

        return await db.ControllerIoSlots.AsNoTracking()
            .Where(s => s.ControllerId == controllerId)
            .OrderBy(s => s.IOType)
            .ThenBy(s => s.SlotNumber)
            .Select(s => new ControllerIoSlotDto(
                s.IOSlotId,
                s.ControllerId,
                s.SlotName,
                s.IOType,
                s.SlotNumber,
                s.IsUsed ?? false,
                s.AssignedPointId
            ))
            .ToListAsync(ct);
    }
}


