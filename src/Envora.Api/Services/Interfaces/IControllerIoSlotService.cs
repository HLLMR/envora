using Envora.Api.Models.Dtos;

namespace Envora.Api.Services.Interfaces;

public interface IControllerIoSlotService
{
    Task<IReadOnlyList<ControllerIoSlotDto>> ListAsync(Guid projectId, Guid controllerId, CancellationToken ct);
}


