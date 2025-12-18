using Envora.Api.Models.Dtos;
using Envora.Api.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Envora.Api.Hubs;

public sealed class ProjectHub : Hub
{
    private readonly INotesService _notesService;
    private readonly ILogger<ProjectHub> _logger;

    public ProjectHub(INotesService notesService, ILogger<ProjectHub> logger)
    {
        _notesService = notesService;
        _logger = logger;
    }

    public async Task SubscribeToProject(Guid projectId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"project:{projectId}");
        _logger.LogInformation("Client {ConnectionId} subscribed to project {ProjectId}", Context.ConnectionId, projectId);
    }

    public async Task UnsubscribeFromProject(Guid projectId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"project:{projectId}");
        _logger.LogInformation("Client {ConnectionId} unsubscribed from project {ProjectId}", Context.ConnectionId, projectId);
    }

    // Client-to-Server: Add Note
    public async Task AddNote(Guid projectId, string discipline, string content, List<Guid>? mentionedUserIds)
    {
        // TODO: Get authorId from authenticated user context
        var authorId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var request = new Envora.Api.Models.Requests.CreateNoteRequest
        {
            Content = content,
            ContentContext = discipline,
            Mentions = mentionedUserIds?.Select(id => id.ToString()).ToList()
        };

        var note = await _notesService.CreateAsync(projectId, authorId, request, CancellationToken.None);

        // Broadcast to all clients in the project group
        await Clients.Group($"project:{projectId}").SendAsync("NoteAdded", note);
    }

    // Client-to-Server: Update Note
    public async Task UpdateNote(Guid noteId, string newContent)
    {
        // TODO: Get userId from authenticated user context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        // Find the note to get projectId
        // For now, we'll need to pass projectId or look it up
        // This is a simplified version - in production, you'd want to pass projectId or look it up
        var request = new Envora.Api.Models.Requests.UpdateNoteRequest { Content = newContent };
        
        // Note: This is a simplified implementation. In production, you'd need to:
        // 1. Look up the note to get projectId, or
        // 2. Pass projectId as a parameter
        // For now, we'll require projectId to be passed
        throw new NotImplementedException("UpdateNote via SignalR requires projectId - use REST API for now");
    }

    // Client-to-Server: Delete Note
    public async Task DeleteNote(Guid projectId, Guid noteId)
    {
        // TODO: Get userId from authenticated user context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var deleted = await _notesService.DeleteAsync(projectId, noteId, userId, CancellationToken.None);

        if (deleted)
        {
            // Broadcast to all clients in the project group
            await Clients.Group($"project:{projectId}").SendAsync("NoteDeleted", noteId);
        }
    }

    // Client-to-Server: Add Reaction
    public async Task AddReaction(Guid projectId, Guid noteId, string emoji)
    {
        // TODO: Get userId from authenticated user context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var request = new Envora.Api.Models.Requests.AddReactionRequest { Emoji = emoji };
        var reactions = await _notesService.AddReactionAsync(projectId, noteId, userId, request, CancellationToken.None);

        // Broadcast updated reactions to all clients in the project group
        await Clients.Group($"project:{projectId}").SendAsync("ReactionAdded", noteId, userId, emoji);
    }

    // Client-to-Server: Remove Reaction (optional, not in spec but useful)
    public async Task RemoveReaction(Guid projectId, Guid noteId, string emoji)
    {
        // TODO: Implement reaction removal if needed
        // For now, this is a placeholder
        _logger.LogInformation("RemoveReaction called for note {NoteId}, emoji {Emoji}", noteId, emoji);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client {ConnectionId} connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client {ConnectionId} disconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}

