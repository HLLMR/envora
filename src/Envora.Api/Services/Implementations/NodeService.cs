using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class NodeService(EnvoraDbContext db) : INodeService
{
    public async Task<IReadOnlyList<NodeDto>> ListAsync(Guid projectId, Guid controllerId, CancellationToken ct)
    {
        return await db.Nodes.AsNoTracking()
            .Where(n => n.ProjectId == projectId && n.ControllerId == controllerId)
            .OrderBy(n => n.NodeName)
            .Select(n => new NodeDto(
                n.NodeId,
                n.ProjectId,
                n.ControllerId,
                n.NodeName,
                n.NodeType,
                n.Protocol,
                n.NetworkAddress,
                n.IsActive ?? true
            ))
            .ToListAsync(ct);
    }

    public async Task<NodeDto?> GetAsync(Guid projectId, Guid controllerId, Guid nodeId, CancellationToken ct)
    {
        return await db.Nodes.AsNoTracking()
            .Where(n => n.ProjectId == projectId && n.ControllerId == controllerId && n.NodeId == nodeId)
            .Select(n => new NodeDto(
                n.NodeId,
                n.ProjectId,
                n.ControllerId,
                n.NodeName,
                n.NodeType,
                n.Protocol,
                n.NetworkAddress,
                n.IsActive ?? true
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<NodeDto> CreateAsync(Guid projectId, Guid controllerId, CreateNodeRequest request, CancellationToken ct)
    {
        var controllerExists = await db.Controllers.AsNoTracking()
            .AnyAsync(c => c.ProjectId == projectId && c.ControllerId == controllerId, ct);
        if (!controllerExists)
        {
            throw new InvalidOperationException("Controller not found for project.");
        }

        var now = DateTime.UtcNow;

        var entity = new Node
        {
            NodeId = Guid.NewGuid(),
            ProjectId = projectId,
            ControllerId = controllerId,
            NodeName = request.NodeName.Trim(),
            NodeType = request.NodeType.Trim(),
            Protocol = request.Protocol,
            NetworkAddress = request.NetworkAddress,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Nodes.Add(entity);
        await db.SaveChangesAsync(ct);

        return new NodeDto(
            entity.NodeId,
            entity.ProjectId,
            entity.ControllerId,
            entity.NodeName,
            entity.NodeType,
            entity.Protocol,
            entity.NetworkAddress,
            entity.IsActive ?? true
        );
    }

    public async Task<NodeDto?> PatchAsync(Guid projectId, Guid controllerId, Guid nodeId, UpdateNodeRequest request, CancellationToken ct)
    {
        var entity = await db.Nodes.SingleOrDefaultAsync(
            n => n.ProjectId == projectId && n.ControllerId == controllerId && n.NodeId == nodeId,
            ct);

        if (entity is null) return null;

        if (request.NodeName is not null) entity.NodeName = request.NodeName.Trim();
        if (request.NodeType is not null) entity.NodeType = request.NodeType.Trim();
        if (request.Protocol is not null) entity.Protocol = request.Protocol;
        if (request.NetworkAddress is not null) entity.NetworkAddress = request.NetworkAddress;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;

        entity.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return new NodeDto(
            entity.NodeId,
            entity.ProjectId,
            entity.ControllerId,
            entity.NodeName,
            entity.NodeType,
            entity.Protocol,
            entity.NetworkAddress,
            entity.IsActive ?? true
        );
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid controllerId, Guid nodeId, CancellationToken ct)
    {
        var entity = await db.Nodes.SingleOrDefaultAsync(
            n => n.ProjectId == projectId && n.ControllerId == controllerId && n.NodeId == nodeId,
            ct);

        if (entity is null) return false;

        db.Nodes.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}


