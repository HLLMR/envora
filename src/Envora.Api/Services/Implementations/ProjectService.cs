using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Models.Shared;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Envora.Api.Services.Implementations;

public sealed class ProjectService(EnvoraDbContext db) : IProjectService
{
    public async Task<PaginatedResponse<ProjectListItemDto>> ListAsync(
        int skip,
        int take,
        string? status,
        Guid? customerId,
        string? searchTerm,
        CancellationToken ct)
    {
        if (skip < 0) skip = 0;
        if (take <= 0) take = 20;
        if (take > 100) take = 100;

        IQueryable<Project> q = db.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(status))
        {
            q = q.Where(p => p.Status == status);
        }

        if (customerId.HasValue)
        {
            q = q.Where(p => p.CustomerId == customerId);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // Simple search per spec: project number or name
            q = q.Where(p => p.ProjectNumber.Contains(searchTerm) || p.ProjectName.Contains(searchTerm));
        }

        var total = await q.CountAsync(ct);

        var data = await q
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(p => new ProjectListItemDto(
                p.ProjectId,
                p.ProjectNumber,
                p.ProjectName,
                p.Status,
                p.CustomerId,
                p.EngineeringFirmId,
                p.StartDate,
                p.EstimatedCompletion,
                p.BudgetAmount
            ))
            .ToListAsync(ct);

        return new PaginatedResponse<ProjectListItemDto>(data, skip, take, total);
    }

    public async Task<ProjectListItemDto?> GetAsync(Guid projectId, CancellationToken ct)
    {
        return await db.Projects.AsNoTracking()
            .Where(p => p.ProjectId == projectId)
            .Select(p => new ProjectListItemDto(
                p.ProjectId,
                p.ProjectNumber,
                p.ProjectName,
                p.Status,
                p.CustomerId,
                p.EngineeringFirmId,
                p.StartDate,
                p.EstimatedCompletion,
                p.BudgetAmount
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<ProjectDetailDto?> GetDetailAsync(Guid projectId, CancellationToken ct)
    {
        var project = await db.Projects.AsNoTracking()
            .Include(p => p.Customer)
            .Include(p => p.EngineeringFirm)
            .Include(p => p.ProjectManager)
            .Include(p => p.DesignEngineer1)
            .Include(p => p.DesignEngineer2)
            .Where(p => p.ProjectId == projectId)
            .SingleOrDefaultAsync(ct);

        if (project == null) return null;

        return new ProjectDetailDto
        {
            ProjectId = project.ProjectId,
            ProjectNumber = project.ProjectNumber,
            ProjectName = project.ProjectName,
            Description = project.Description,
            Status = project.Status,
            CustomerId = project.CustomerId,
            CustomerName = project.Customer?.CompanyName,
            EngineeringFirmId = project.EngineeringFirmId,
            EngineeringFirmName = project.EngineeringFirm?.CompanyName,
            StartDate = project.StartDate,
            EstimatedCompletion = project.EstimatedCompletion,
            ActualCompletion = project.ActualCompletion,
            BudgetAmount = project.BudgetAmount,
            ProjectManagerId = project.ProjectManagerId,
            ProjectManagerName = project.ProjectManager != null 
                ? $"{project.ProjectManager.FirstName} {project.ProjectManager.LastName}".Trim()
                : null,
            ProjectManagerEmail = project.ProjectManager?.Email,
            DesignEngineer1Id = project.DesignEngineer1Id,
            DesignEngineer1Name = project.DesignEngineer1 != null
                ? $"{project.DesignEngineer1.FirstName} {project.DesignEngineer1.LastName}".Trim()
                : null,
            DesignEngineer1Email = project.DesignEngineer1?.Email,
            DesignEngineer2Id = project.DesignEngineer2Id,
            DesignEngineer2Name = project.DesignEngineer2 != null
                ? $"{project.DesignEngineer2.FirstName} {project.DesignEngineer2.LastName}".Trim()
                : null,
            DesignEngineer2Email = project.DesignEngineer2?.Email,
            Location = project.Location,
            BuildingType = project.BuildingType,
            SquareFootage = project.SquareFootage,
            Notes = project.Notes,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<ProjectListItemDto> CreateAsync(CreateProjectRequest request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var entity = new Project
        {
            ProjectId = Guid.NewGuid(),
            ProjectNumber = request.ProjectNumber.Trim(),
            ProjectName = request.ProjectName.Trim(),
            Description = request.Description,
            CustomerId = request.CustomerId,
            EngineeringFirmId = request.EngineeringFirmId,
            Status = request.Status.Trim(),
            StartDate = request.StartDate,
            EstimatedCompletion = request.EstimatedCompletion,
            BudgetAmount = request.BudgetAmount,
            ProjectManagerId = request.ProjectManagerId,
            DesignEngineer1Id = request.DesignEngineer1Id,
            DesignEngineer2Id = request.DesignEngineer2Id,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedByUserId = null
        };

        db.Projects.Add(entity);
        await db.SaveChangesAsync(ct);

        return new ProjectListItemDto(
            entity.ProjectId,
            entity.ProjectNumber,
            entity.ProjectName,
            entity.Status,
            entity.CustomerId,
            entity.EngineeringFirmId,
            entity.StartDate,
            entity.EstimatedCompletion,
            entity.BudgetAmount
        );
    }

    public async Task<ProjectListItemDto?> UpdateAsync(Guid projectId, UpdateProjectRequest request, CancellationToken ct)
    {
        var entity = await db.Projects.SingleOrDefaultAsync(p => p.ProjectId == projectId, ct);
        if (entity is null) return null;

        entity.ProjectName = request.ProjectName.Trim();
        entity.Description = request.Description;
        entity.CustomerId = request.CustomerId;
        entity.EngineeringFirmId = request.EngineeringFirmId;
        entity.Status = request.Status.Trim();
        entity.StartDate = request.StartDate;
        entity.EstimatedCompletion = request.EstimatedCompletion;
        entity.ActualCompletion = request.ActualCompletion;
        entity.BudgetAmount = request.BudgetAmount;
        entity.ProjectManagerId = request.ProjectManagerId;
        entity.DesignEngineer1Id = request.DesignEngineer1Id;
        entity.DesignEngineer2Id = request.DesignEngineer2Id;
        entity.Location = request.Location;
        entity.BuildingType = request.BuildingType;
        entity.SquareFootage = request.SquareFootage;
        entity.Notes = request.Notes;
        entity.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return new ProjectListItemDto(
            entity.ProjectId,
            entity.ProjectNumber,
            entity.ProjectName,
            entity.Status,
            entity.CustomerId,
            entity.EngineeringFirmId,
            entity.StartDate,
            entity.EstimatedCompletion,
            entity.BudgetAmount
        );
    }

    public async Task<bool> DeleteAsync(Guid projectId, CancellationToken ct)
    {
        var entity = await db.Projects.SingleOrDefaultAsync(p => p.ProjectId == projectId, ct);
        if (entity is null) return false;

        db.Projects.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct)
    {
        var projects = await db.Projects.AsNoTracking().ToListAsync(ct);
        var equipment = await db.Equipment.AsNoTracking().CountAsync(ct);
        var points = await db.Points.AsNoTracking().CountAsync(ct);

        var projectsByStatus = projects
            .GroupBy(p => p.Status ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());

        var activeProjects = projects.Count(p => 
            p.Status != null && 
            (p.Status.Equals("Active", StringComparison.OrdinalIgnoreCase) || 
             p.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase)));

        var completedProjects = projects.Count(p => 
            p.Status != null && 
            p.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase));

        var onHoldProjects = projects.Count(p => 
            p.Status != null && 
            p.Status.Equals("On Hold", StringComparison.OrdinalIgnoreCase));

        return new DashboardStatsDto
        {
            TotalProjects = projects.Count,
            ActiveProjects = activeProjects,
            CompletedProjects = completedProjects,
            OnHoldProjects = onHoldProjects,
            ProjectsByStatus = projectsByStatus,
            TotalEquipment = equipment,
            TotalPoints = points
        };
    }
}


