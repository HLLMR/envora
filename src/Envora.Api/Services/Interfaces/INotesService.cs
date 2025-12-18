using Envora.Api.Models.Dtos;
using Envora.Api.Models.Requests;
using Envora.Api.Models.Shared;

namespace Envora.Api.Services.Interfaces;

public interface INotesService
{
    Task<PaginatedResponse<NoteDto>> ListAsync(
        Guid projectId,
        int skip,
        int take,
        string? discipline,
        string? disciplineTab,
        CancellationToken ct);

    Task<NoteDto?> GetAsync(Guid projectId, Guid noteId, CancellationToken ct);

    Task<NoteDto> CreateAsync(Guid projectId, Guid authorId, CreateNoteRequest request, CancellationToken ct);

    Task<NoteDto?> UpdateAsync(Guid projectId, Guid noteId, Guid userId, UpdateNoteRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid projectId, Guid noteId, Guid userId, CancellationToken ct);

    Task<Dictionary<string, int>> AddReactionAsync(Guid projectId, Guid noteId, Guid userId, AddReactionRequest request, CancellationToken ct);
}

