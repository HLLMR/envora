using Envora.Web.Models.Notes;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

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

    Task<NoteDto> CreateAsync(Guid projectId, CreateNoteRequest request, CancellationToken ct);

    Task<NoteDto?> UpdateAsync(Guid projectId, Guid noteId, UpdateNoteRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid projectId, Guid noteId, CancellationToken ct);

    Task<Dictionary<string, int>> AddReactionAsync(Guid projectId, Guid noteId, AddReactionRequest request, CancellationToken ct);
}

