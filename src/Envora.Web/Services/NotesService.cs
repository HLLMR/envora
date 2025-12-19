using System.Net;
using System.Net.Http.Json;
using Envora.Web.Models.Notes;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public sealed class NotesService(HttpClient http) : INotesService
{
    private static async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
            return result ?? throw new ApiException("Invalid response from server", (int)response.StatusCode);
        }

        await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
        throw new InvalidOperationException("Unreachable");
    }

    public async Task<PaginatedResponse<NoteDto>> ListAsync(
        Guid projectId,
        int skip,
        int take,
        string? discipline,
        string? disciplineTab,
        CancellationToken ct)
    {
        try
        {
            var url = $"api/v1/projects/{projectId}/notes?skip={skip}&take={take}";
            if (!string.IsNullOrWhiteSpace(discipline)) url += $"&discipline={Uri.EscapeDataString(discipline)}";
            if (!string.IsNullOrWhiteSpace(disciplineTab)) url += $"&disciplineTab={Uri.EscapeDataString(disciplineTab)}";

            var response = await http.GetAsync(url, ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<NoteDto>>(cancellationToken: ct);
                return result ?? new PaginatedResponse<NoteDto>();
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return new PaginatedResponse<NoteDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<NoteDto?> GetAsync(Guid projectId, Guid noteId, CancellationToken ct)
    {
        try
        {
            var response = await http.GetAsync($"api/v1/projects/{projectId}/notes/{noteId}", ct);
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<NoteDto>(cancellationToken: ct);
            }

            await ApiErrorHelper.ThrowApiExceptionAsync(response, ct);
            return null;
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<NoteDto> CreateAsync(Guid projectId, CreateNoteRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PostAsJsonAsync($"api/v1/projects/{projectId}/notes", request, ct);
            return await HandleResponseAsync<NoteDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<NoteDto?> UpdateAsync(Guid projectId, Guid noteId, UpdateNoteRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PatchAsJsonAsync($"api/v1/projects/{projectId}/notes/{noteId}", request, ct);
            if (res.StatusCode == HttpStatusCode.NotFound) return null;
            return await HandleResponseAsync<NoteDto>(res, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<bool> DeleteAsync(Guid projectId, Guid noteId, CancellationToken ct)
    {
        try
        {
            var res = await http.DeleteAsync($"api/v1/projects/{projectId}/notes/{noteId}", ct);
            if (!res.IsSuccessStatusCode && res.StatusCode != HttpStatusCode.NotFound)
            {
                await ApiErrorHelper.ThrowApiExceptionAsync(res, ct);
            }
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    public async Task<Dictionary<string, int>> AddReactionAsync(Guid projectId, Guid noteId, AddReactionRequest request, CancellationToken ct)
    {
        try
        {
            var res = await http.PostAsJsonAsync($"api/v1/projects/{projectId}/notes/{noteId}/reactions", request, ct);
            var result = await HandleResponseAsync<ReactionResponse>(res, ct);
            return result.Reactions;
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException($"Network error: {ex.Message}. Please check if the API is running.", 0);
        }
    }

    private sealed class ReactionResponse
    {
        public Guid NoteId { get; set; }
        public Dictionary<string, int> Reactions { get; set; } = new();
    }
}

