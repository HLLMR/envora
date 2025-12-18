using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class ControllerService(EnvoraDbContext db) : IControllerService
{
    public async Task<IReadOnlyList<ControllerDto>> ListByProjectAsync(Guid projectId, CancellationToken ct)
    {
        return await db.Controllers.AsNoTracking()
            .Where(c => c.ProjectId == projectId)
            .OrderBy(c => c.ControllerName)
            .Select(c => new ControllerDto(
                c.ControllerId,
                c.ProjectId,
                c.ControllerName,
                c.ControllerType,
                c.Manufacturer,
                c.Model,
                c.IsActive
            ))
            .ToListAsync(ct);
    }

    public async Task<ControllerDto?> GetAsync(Guid projectId, Guid controllerId, CancellationToken ct)
    {
        return await db.Controllers.AsNoTracking()
            .Where(c => c.ProjectId == projectId && c.ControllerId == controllerId)
            .Select(c => new ControllerDto(
                c.ControllerId,
                c.ProjectId,
                c.ControllerName,
                c.ControllerType,
                c.Manufacturer,
                c.Model,
                c.IsActive
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<ControllerDto> CreateAsync(Guid projectId, CreateControllerRequest request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var entity = new Controller
        {
            ControllerId = Guid.NewGuid(),
            ProjectId = projectId,
            ControllerName = request.ControllerName.Trim(),
            ControllerType = request.ControllerType.Trim(),
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Controllers.Add(entity);
        await db.SaveChangesAsync(ct);

        return new ControllerDto(
            entity.ControllerId,
            entity.ProjectId,
            entity.ControllerName,
            entity.ControllerType,
            entity.Manufacturer,
            entity.Model,
            entity.IsActive
        );
    }

    public async Task<ControllerDto?> PatchAsync(Guid projectId, Guid controllerId, UpdateControllerRequest request, CancellationToken ct)
    {
        var entity = await db.Controllers.SingleOrDefaultAsync(
            c => c.ProjectId == projectId && c.ControllerId == controllerId,
            ct);

        if (entity is null) return null;

        if (request.ControllerName is not null) entity.ControllerName = request.ControllerName.Trim();
        if (request.ControllerType is not null) entity.ControllerType = request.ControllerType.Trim();
        if (request.Manufacturer is not null) entity.Manufacturer = request.Manufacturer;
        if (request.Model is not null) entity.Model = request.Model;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;

        entity.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return new ControllerDto(
            entity.ControllerId,
            entity.ProjectId,
            entity.ControllerName,
            entity.ControllerType,
            entity.Manufacturer,
            entity.Model,
            entity.IsActive
        );
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid controllerId, CancellationToken ct)
    {
        var entity = await db.Controllers.SingleOrDefaultAsync(
            c => c.ProjectId == projectId && c.ControllerId == controllerId,
            ct);

        if (entity is null) return false;

        db.Controllers.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}


