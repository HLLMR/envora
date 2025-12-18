using Envora.Api.Hubs;
using Envora.Api.Models.Requests;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Envora.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/notes")]
public sealed class NotesController(INotesService notes, IHubContext<ProjectHub> hubContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(
        [FromRoute] Guid projectId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        [FromQuery] string? discipline = null,
        [FromQuery] string? disciplineTab = null,
        CancellationToken ct = default)
    {
        var result = await notes.ListAsync(projectId, skip, take, discipline, disciplineTab, ct);
        return Ok(result);
    }

    [HttpGet("{noteId:guid}")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid projectId,
        [FromRoute] Guid noteId,
        CancellationToken ct)
    {
        var note = await notes.GetAsync(projectId, noteId, ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromRoute] Guid projectId,
        [FromBody] CreateNoteRequest request,
        CancellationToken ct)
    {
        // TODO: Get authorId from authenticated user context
        // For now, use a temporary default user ID (will be replaced with auth)
        var authorId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var created = await notes.CreateAsync(projectId, authorId, request, ct);
        
        // Broadcast SignalR event
        await hubContext.Clients.Group($"project:{projectId}").SendAsync("NoteAdded", created);
        
        return CreatedAtAction(nameof(Get), new { projectId, noteId = created.NoteId }, created);
    }

    [HttpPatch("{noteId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectId,
        [FromRoute] Guid noteId,
        [FromBody] UpdateNoteRequest request,
        CancellationToken ct)
    {
        // TODO: Get userId from authenticated user context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        try
        {
            var updated = await notes.UpdateAsync(projectId, noteId, userId, request, ct);
            if (updated != null)
            {
                // Broadcast SignalR event
                await hubContext.Clients.Group($"project:{projectId}").SendAsync("NoteUpdated", updated);
                return Ok(updated);
            }
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{noteId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid projectId,
        [FromRoute] Guid noteId,
        CancellationToken ct)
    {
        // TODO: Get userId from authenticated user context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        try
        {
            var deleted = await notes.DeleteAsync(projectId, noteId, userId, ct);
            if (deleted)
            {
                // Broadcast SignalR event
                await hubContext.Clients.Group($"project:{projectId}").SendAsync("NoteDeleted", noteId);
                return NoContent();
            }
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("{noteId:guid}/reactions")]
    public async Task<IActionResult> AddReaction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid noteId,
        [FromBody] AddReactionRequest request,
        CancellationToken ct)
    {
        // TODO: Get userId from authenticated user context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        try
        {
            var reactions = await notes.AddReactionAsync(projectId, noteId, userId, request, ct);
            
            // Broadcast SignalR event
            await hubContext.Clients.Group($"project:{projectId}").SendAsync("ReactionAdded", noteId, userId, request.Emoji);
            
            return Ok(new { noteId, reactions });
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}

