using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;

namespace Envora.Api.Services.Interfaces;

public interface IDeviceService
{
    Task<IReadOnlyList<DeviceDto>> ListByProjectAsync(Guid projectId, CancellationToken ct);

    Task<DeviceDto?> GetAsync(Guid projectId, Guid deviceId, CancellationToken ct);

    Task<DeviceDto> CreateAsync(Guid projectId, CreateDeviceRequest request, CancellationToken ct);

    Task<DeviceDto?> PatchAsync(Guid projectId, Guid deviceId, UpdateDeviceRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid projectId, Guid deviceId, CancellationToken ct);
}


