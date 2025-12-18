using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class EquipmentService(EnvoraDbContext db) : IEquipmentService
{
    public async Task<IReadOnlyList<EquipmentDto>> ListByProjectAsync(Guid projectId, CancellationToken ct)
    {
        return await db.Equipment.AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .OrderBy(e => e.EquipmentTag)
            .Select(e => new EquipmentDto(
                e.EquipmentId,
                e.ProjectId,
                e.EquipmentTag,
                e.EquipmentType,
                e.Manufacturer,
                e.Model,
                e.Location,
                e.Description
            ))
            .ToListAsync(ct);
    }

    public async Task<EquipmentDto?> GetAsync(Guid projectId, Guid equipmentId, CancellationToken ct)
    {
        return await db.Equipment.AsNoTracking()
            .Where(e => e.ProjectId == projectId && e.EquipmentId == equipmentId)
            .Select(e => new EquipmentDto(
                e.EquipmentId,
                e.ProjectId,
                e.EquipmentTag,
                e.EquipmentType,
                e.Manufacturer,
                e.Model,
                e.Location,
                e.Description
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<EquipmentDto> CreateAsync(Guid projectId, CreateEquipmentRequest request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var entity = new Equipment
        {
            EquipmentId = Guid.NewGuid(),
            ProjectId = projectId,
            EquipmentTag = request.EquipmentTag.Trim(),
            EquipmentType = request.EquipmentType.Trim(),
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            Location = request.Location,
            Description = request.Description,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Equipment.Add(entity);
        await db.SaveChangesAsync(ct);

        return new EquipmentDto(
            entity.EquipmentId,
            entity.ProjectId,
            entity.EquipmentTag,
            entity.EquipmentType,
            entity.Manufacturer,
            entity.Model,
            entity.Location,
            entity.Description
        );
    }

    public async Task<EquipmentDto?> PatchAsync(Guid projectId, Guid equipmentId, UpdateEquipmentRequest request, CancellationToken ct)
    {
        var entity = await db.Equipment.SingleOrDefaultAsync(
            e => e.ProjectId == projectId && e.EquipmentId == equipmentId,
            ct);

        if (entity is null) return null;

        if (request.EquipmentTag is not null) entity.EquipmentTag = request.EquipmentTag.Trim();
        if (request.EquipmentType is not null) entity.EquipmentType = request.EquipmentType.Trim();
        if (request.Manufacturer is not null) entity.Manufacturer = request.Manufacturer;
        if (request.Model is not null) entity.Model = request.Model;
        if (request.Location is not null) entity.Location = request.Location;
        if (request.Description is not null) entity.Description = request.Description;

        entity.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return new EquipmentDto(
            entity.EquipmentId,
            entity.ProjectId,
            entity.EquipmentTag,
            entity.EquipmentType,
            entity.Manufacturer,
            entity.Model,
            entity.Location,
            entity.Description
        );
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, CancellationToken ct)
    {
        var entity = await db.Equipment.SingleOrDefaultAsync(
            e => e.ProjectId == projectId && e.EquipmentId == equipmentId,
            ct);

        if (entity is null) return false;

        db.Equipment.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}


