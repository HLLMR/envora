using System.Net.Http.Json;
using Envora.Web.Models.Notes;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class NotesService(HttpClient http) : INotesService
{
    public async Task<PaginatedResponse<NoteDto>> ListAsync(
        Guid projectId,
        int skip,
        int take,
        string? discipline,
        string? disciplineTab,
        CancellationToken ct)
    {
        var url = $"api/v1/projects/{projectId}/notes?skip={skip}&take={take}";
        if (!string.IsNullOrWhiteSpace(discipline)) url += $"&discipline={Uri.EscapeDataString(discipline)}";
        if (!string.IsNullOrWhiteSpace(disciplineTab)) url += $"&disciplineTab={Uri.EscapeDataString(disciplineTab)}";

        var result = await http.GetFromJsonAsync<PaginatedResponse<NoteDto>>(url, ct);
        return result ?? new PaginatedResponse<NoteDto>();
    }

    public async Task<NoteDto?> GetAsync(Guid projectId, Guid noteId, CancellationToken ct)
    {
        return await http.GetFromJsonAsync<NoteDto>($"api/v1/projects/{projectId}/notes/{noteId}", ct);
    }

    public async Task<NoteDto> CreateAsync(Guid projectId, CreateNoteRequest request, CancellationToken ct)
    {
        var res = await http.PostAsJsonAsync($"api/v1/projects/{projectId}/notes", request, ct);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<NoteDto>(cancellationToken: ct))!;
    }

    public async Task<NoteDto?> UpdateAsync(Guid projectId, Guid noteId, UpdateNoteRequest request, CancellationToken ct)
    {
        var res = await http.PatchAsJsonAsync($"api/v1/projects/{projectId}/notes/{noteId}", request, ct);
        if (res.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<NoteDto>(cancellationToken: ct);
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid noteId, CancellationToken ct)
    {
        var res = await http.DeleteAsync($"api/v1/projects/{projectId}/notes/{noteId}", ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<Dictionary<string, int>> AddReactionAsync(Guid projectId, Guid noteId, AddReactionRequest request, CancellationToken ct)
    {
        var res = await http.PostAsJsonAsync($"api/v1/projects/{projectId}/notes/{noteId}/reactions", request, ct);
        res.EnsureSuccessStatusCode();
        var result = await res.Content.ReadFromJsonAsync<ReactionResponse>(cancellationToken: ct);
        return result?.Reactions ?? new Dictionary<string, int>();
    }

    private sealed class ReactionResponse
    {
        public Guid NoteId { get; set; }
        public Dictionary<string, int> Reactions { get; set; } = new();
    }
}

