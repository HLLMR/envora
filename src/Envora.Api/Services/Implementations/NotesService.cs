using Envora.Api.Data;
using Envora.Api.Data.Entities;
using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Models.Shared;
using Envora.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Services.Implementations;

public sealed class NotesService(EnvoraDbContext db) : INotesService
{
    public async Task<PaginatedResponse<NoteDto>> ListAsync(
        Guid projectId,
        int skip,
        int take,
        string? discipline,
        string? disciplineTab,
        CancellationToken ct)
    {
        if (skip < 0) skip = 0;
        if (take <= 0) take = 50;
        if (take > 100) take = 100;

        IQueryable<Note> q = db.Notes
            .AsNoTracking()
            .Where(n => n.ProjectId == projectId && n.DeletedAt == null);

        // Filter by discipline if provided
        if (!string.IsNullOrWhiteSpace(discipline))
        {
            q = q.Where(n => n.Discipline == discipline);
        }

        // Note: disciplineTab filtering is not fully supported yet since we only store Discipline in the DB
        // ContentContext format in DTOs is "DISCIPLINE" or "DISCIPLINE:TabName" but DB only has Discipline
        // For now, we filter by discipline only; tab filtering can be added later if needed

        var total = await q.CountAsync(ct);

        var notes = await q
            .Include(n => n.Author)
            .Include(n => n.ParentNote)
            .OrderByDescending(n => n.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);

        // Get reply counts and reactions for each note
        var noteIds = notes.Select(n => n.NoteId).ToList();
        var replyCounts = await db.Notes
            .AsNoTracking()
            .Where(n => n.ParentNoteId != null && noteIds.Contains(n.ParentNoteId.Value) && n.DeletedAt == null)
            .GroupBy(n => n.ParentNoteId)
            .Select(g => new { ParentNoteId = g.Key!.Value, Count = g.Count() })
            .ToListAsync(ct);

        var reactions = await db.NoteReactions
            .AsNoTracking()
            .Where(r => noteIds.Contains(r.NoteId))
            .GroupBy(r => new { r.NoteId, r.Emoji })
            .Select(g => new { g.Key.NoteId, g.Key.Emoji, Count = g.Count() })
            .ToListAsync(ct);

        var data = notes.Select(n =>
        {
            var replyCount = replyCounts.FirstOrDefault(rc => rc.ParentNoteId == n.NoteId)?.Count ?? 0;
            var noteReactions = reactions
                .Where(r => r.NoteId == n.NoteId)
                .ToDictionary(r => r.Emoji, r => r.Count);

            // Build contentContext from Discipline (format: "DISCIPLINE:TabName" or just "DISCIPLINE")
            var contentContext = n.Discipline != null ? n.Discipline.ToUpperInvariant() : null;

            return new NoteDto
            {
                NoteId = n.NoteId,
                ProjectId = n.ProjectId,
                ContentContext = contentContext,
                Content = n.Content,
                Author = n.Author.Email ?? "unknown",
                AuthorName = $"{n.Author.FirstName} {n.Author.LastName}".Trim(),
                CreatedAt = n.CreatedAt,
                EditedAt = n.UpdatedAt > n.CreatedAt ? n.UpdatedAt : null,
                ParentNoteId = n.ParentNoteId,
                Replies = replyCount,
                Reactions = noteReactions
            };
        }).ToList();

        return new PaginatedResponse<NoteDto>(data, skip, take, total);
    }

    public async Task<NoteDto?> GetAsync(Guid projectId, Guid noteId, CancellationToken ct)
    {
        var note = await db.Notes
            .AsNoTracking()
            .Include(n => n.Author)
            .Where(n => n.ProjectId == projectId && n.NoteId == noteId && n.DeletedAt == null)
            .SingleOrDefaultAsync(ct);

        if (note == null) return null;

        var replyCount = await db.Notes
            .AsNoTracking()
            .CountAsync(n => n.ParentNoteId == noteId && n.DeletedAt == null, ct);

        var reactions = await db.NoteReactions
            .AsNoTracking()
            .Where(r => r.NoteId == noteId)
            .GroupBy(r => r.Emoji)
            .Select(g => new { Emoji = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var contentContext = note.Discipline != null ? note.Discipline.ToUpperInvariant() : null;

        return new NoteDto
        {
            NoteId = note.NoteId,
            ProjectId = note.ProjectId,
            ContentContext = contentContext,
            Content = note.Content,
            Author = note.Author.Email ?? note.Author.Username ?? "unknown",
            AuthorName = $"{note.Author.FirstName} {note.Author.LastName}".Trim(),
            CreatedAt = note.CreatedAt,
            EditedAt = note.UpdatedAt > note.CreatedAt ? note.UpdatedAt : null,
            ParentNoteId = note.ParentNoteId,
            Replies = replyCount,
            Reactions = reactions.ToDictionary(r => r.Emoji, r => r.Count)
        };
    }

    public async Task<NoteDto> CreateAsync(Guid projectId, Guid authorId, CreateNoteRequest request, CancellationToken ct)
    {
        // Parse contentContext to extract discipline
        // Format: "DISCIPLINE:TabName" or just "DISCIPLINE"
        string? discipline = null;
        if (!string.IsNullOrWhiteSpace(request.ContentContext))
        {
            var parts = request.ContentContext.Split(':', 2);
            discipline = parts[0].Trim();
        }

        var now = DateTime.UtcNow;

        // Parse mentions from content or request.Mentions
        var mentionedUserIds = request.Mentions != null && request.Mentions.Any()
            ? string.Join(",", request.Mentions)
            : null;

        var entity = new Note
        {
            NoteId = Guid.NewGuid(),
            ProjectId = projectId,
            Discipline = discipline,
            ParentNoteId = request.ParentNoteId,
            AuthorId = authorId,
            Content = request.Content.Trim(),
            MentionedUserIds = mentionedUserIds,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Notes.Add(entity);
        await db.SaveChangesAsync(ct);

        return await GetAsync(projectId, entity.NoteId, ct) ?? throw new InvalidOperationException("Failed to retrieve created note");
    }

    public async Task<NoteDto?> UpdateAsync(Guid projectId, Guid noteId, Guid userId, UpdateNoteRequest request, CancellationToken ct)
    {
        var note = await db.Notes
            .Where(n => n.ProjectId == projectId && n.NoteId == noteId && n.DeletedAt == null)
            .SingleOrDefaultAsync(ct);

        if (note == null) return null;

        // Only author can edit (for now; admin check can be added later)
        if (note.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("Only the note author can edit this note");
        }

        note.Content = request.Content.Trim();
        note.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return await GetAsync(projectId, noteId, ct);
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid noteId, Guid userId, CancellationToken ct)
    {
        var note = await db.Notes
            .Where(n => n.ProjectId == projectId && n.NoteId == noteId && n.DeletedAt == null)
            .SingleOrDefaultAsync(ct);

        if (note == null) return false;

        // Only author can delete (for now; admin check can be added later)
        if (note.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("Only the note author can delete this note");
        }

        // Soft delete
        note.DeletedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<Dictionary<string, int>> AddReactionAsync(Guid projectId, Guid noteId, Guid userId, AddReactionRequest request, CancellationToken ct)
    {
        var note = await db.Notes
            .Where(n => n.ProjectId == projectId && n.NoteId == noteId && n.DeletedAt == null)
            .SingleOrDefaultAsync(ct);

        if (note == null)
        {
            throw new InvalidOperationException("Note not found");
        }

        // Check if reaction already exists
        var existing = await db.NoteReactions
            .Where(r => r.NoteId == noteId && r.UserId == userId && r.Emoji == request.Emoji)
            .SingleOrDefaultAsync(ct);

        if (existing == null)
        {
            var reaction = new NoteReaction
            {
                ReactionId = Guid.NewGuid(),
                NoteId = noteId,
                UserId = userId,
                Emoji = request.Emoji.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            db.NoteReactions.Add(reaction);
            await db.SaveChangesAsync(ct);
        }

        // Return updated reaction counts
        var reactions = await db.NoteReactions
            .AsNoTracking()
            .Where(r => r.NoteId == noteId)
            .GroupBy(r => r.Emoji)
            .Select(g => new { Emoji = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        return reactions.ToDictionary(r => r.Emoji, r => r.Count);
    }
}

