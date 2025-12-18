using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class DeviceService(EnvoraDbContext db) : IDeviceService
{
    public async Task<IReadOnlyList<DeviceDto>> ListByProjectAsync(Guid projectId, CancellationToken ct)
    {
        return await db.Devices.AsNoTracking()
            .Where(d => d.ProjectId == projectId)
            .OrderBy(d => d.DeviceName)
            .Select(d => new DeviceDto(
                d.DeviceId,
                d.ProjectId,
                d.DeviceName,
                d.DeviceType,
                d.Category,
                d.MountedOnEquipmentId,
                d.Manufacturer,
                d.Model,
                d.IsActive
            ))
            .ToListAsync(ct);
    }

    public async Task<DeviceDto?> GetAsync(Guid projectId, Guid deviceId, CancellationToken ct)
    {
        return await db.Devices.AsNoTracking()
            .Where(d => d.ProjectId == projectId && d.DeviceId == deviceId)
            .Select(d => new DeviceDto(
                d.DeviceId,
                d.ProjectId,
                d.DeviceName,
                d.DeviceType,
                d.Category,
                d.MountedOnEquipmentId,
                d.Manufacturer,
                d.Model,
                d.IsActive
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<DeviceDto> CreateAsync(Guid projectId, CreateDeviceRequest request, CancellationToken ct)
    {
        if (request.MountedOnEquipmentId.HasValue)
        {
            var exists = await db.Equipment.AsNoTracking()
                .AnyAsync(e => e.ProjectId == projectId && e.EquipmentId == request.MountedOnEquipmentId, ct);
            if (!exists)
            {
                throw new InvalidOperationException("MountedOnEquipmentId not found for project.");
            }
        }

        var now = DateTime.UtcNow;

        var entity = new Device
        {
            DeviceId = Guid.NewGuid(),
            ProjectId = projectId,
            DeviceName = request.DeviceName.Trim(),
            DeviceType = request.DeviceType.Trim(),
            Category = request.Category,
            MountedOnEquipmentId = request.MountedOnEquipmentId,
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Devices.Add(entity);
        await db.SaveChangesAsync(ct);

        return new DeviceDto(
            entity.DeviceId,
            entity.ProjectId,
            entity.DeviceName,
            entity.DeviceType,
            entity.Category,
            entity.MountedOnEquipmentId,
            entity.Manufacturer,
            entity.Model,
            entity.IsActive
        );
    }

    public async Task<DeviceDto?> PatchAsync(Guid projectId, Guid deviceId, UpdateDeviceRequest request, CancellationToken ct)
    {
        var entity = await db.Devices.SingleOrDefaultAsync(
            d => d.ProjectId == projectId && d.DeviceId == deviceId,
            ct);

        if (entity is null) return null;

        if (request.MountedOnEquipmentId.HasValue)
        {
            var exists = await db.Equipment.AsNoTracking()
                .AnyAsync(e => e.ProjectId == projectId && e.EquipmentId == request.MountedOnEquipmentId, ct);
            if (!exists)
            {
                throw new InvalidOperationException("MountedOnEquipmentId not found for project.");
            }
        }

        if (request.DeviceName is not null) entity.DeviceName = request.DeviceName.Trim();
        if (request.DeviceType is not null) entity.DeviceType = request.DeviceType.Trim();
        if (request.Category is not null) entity.Category = request.Category;
        if (request.MountedOnEquipmentId is not null) entity.MountedOnEquipmentId = request.MountedOnEquipmentId;
        if (request.Manufacturer is not null) entity.Manufacturer = request.Manufacturer;
        if (request.Model is not null) entity.Model = request.Model;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;

        entity.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return new DeviceDto(
            entity.DeviceId,
            entity.ProjectId,
            entity.DeviceName,
            entity.DeviceType,
            entity.Category,
            entity.MountedOnEquipmentId,
            entity.Manufacturer,
            entity.Model,
            entity.IsActive
        );
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid deviceId, CancellationToken ct)
    {
        var entity = await db.Devices.SingleOrDefaultAsync(
            d => d.ProjectId == projectId && d.DeviceId == deviceId,
            ct);

        if (entity is null) return false;

        db.Devices.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}


