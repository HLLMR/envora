using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class PointService(EnvoraDbContext db) : IPointService
{
    public async Task<IReadOnlyList<PointDto>> ListByEquipmentAsync(Guid projectId, Guid equipmentId, CancellationToken ct)
    {
        return await db.Points.AsNoTracking()
            .Where(p => p.ProjectId == projectId && p.EquipmentId == equipmentId)
            .OrderBy(p => p.PointTag)
            .Select(p => new PointDto(
                p.PointId,
                p.ProjectId,
                p.EquipmentId!.Value,
                p.PointTag,
                p.SourceType,
                p.PointType,
                p.DataType,
                p.Unit,
                p.PointDescription
            ))
            .ToListAsync(ct);
    }

    public async Task<PointDto?> GetAsync(Guid projectId, Guid equipmentId, Guid pointId, CancellationToken ct)
    {
        return await db.Points.AsNoTracking()
            .Where(p => p.ProjectId == projectId && p.EquipmentId == equipmentId && p.PointId == pointId)
            .Select(p => new PointDto(
                p.PointId,
                p.ProjectId,
                p.EquipmentId!.Value,
                p.PointTag,
                p.SourceType,
                p.PointType,
                p.DataType,
                p.Unit,
                p.PointDescription
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<PointDto> CreateAsync(Guid projectId, Guid equipmentId, CreatePointRequest request, CancellationToken ct)
    {
        // Validate equipment exists and belongs to project
        var equipmentExists = await db.Equipment.AsNoTracking()
            .AnyAsync(e => e.ProjectId == projectId && e.EquipmentId == equipmentId, ct);
        if (!equipmentExists)
        {
            throw new InvalidOperationException("Equipment not found for project.");
        }

        var now = DateTime.UtcNow;

        var entity = new Point
        {
            PointId = Guid.NewGuid(),
            ProjectId = projectId,
            EquipmentId = equipmentId,
            PointTag = request.PointTag.Trim(),
            PointDescription = request.PointDescription,
            SourceType = request.SourceType.Trim(),
            PointType = request.PointType.Trim(),
            DataType = request.DataType.Trim(),
            Unit = request.Unit,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Points.Add(entity);
        await db.SaveChangesAsync(ct);

        return new PointDto(
            entity.PointId,
            entity.ProjectId,
            entity.EquipmentId!.Value,
            entity.PointTag,
            entity.SourceType,
            entity.PointType,
            entity.DataType,
            entity.Unit,
            entity.PointDescription
        );
    }

    public async Task<PointDto?> PatchAsync(Guid projectId, Guid equipmentId, Guid pointId, UpdatePointRequest request, CancellationToken ct)
    {
        var entity = await db.Points.SingleOrDefaultAsync(
            p => p.ProjectId == projectId && p.EquipmentId == equipmentId && p.PointId == pointId,
            ct);

        if (entity is null) return null;

        if (request.PointDescription is not null) entity.PointDescription = request.PointDescription;
        if (request.PointType is not null) entity.PointType = request.PointType.Trim();
        if (request.DataType is not null) entity.DataType = request.DataType.Trim();
        if (request.Unit is not null) entity.Unit = request.Unit;

        entity.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return new PointDto(
            entity.PointId,
            entity.ProjectId,
            entity.EquipmentId!.Value,
            entity.PointTag,
            entity.SourceType,
            entity.PointType,
            entity.DataType,
            entity.Unit,
            entity.PointDescription
        );
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid equipmentId, Guid pointId, CancellationToken ct)
    {
        var entity = await db.Points.SingleOrDefaultAsync(
            p => p.ProjectId == projectId && p.EquipmentId == equipmentId && p.PointId == pointId,
            ct);

        if (entity is null) return false;

        db.Points.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}


