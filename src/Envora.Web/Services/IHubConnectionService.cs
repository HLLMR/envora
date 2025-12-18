using Envora.Web.Models.Notes;

namespace Envora.Web.Services;

public interface IHubConnectionService
{
    bool IsConnected { get; }

    // Events
    event Action<NoteDto>? OnNoteAdded;
    event Action<NoteDto>? OnNoteUpdated;
    event Action<Guid>? OnNoteDeleted;
    event Action<Guid, Guid, string>? OnReactionAdded;

    // Methods
    Task ConnectAsync();
    Task DisconnectAsync();
    Task SubscribeToProjectAsync(Guid projectId);
    Task UnsubscribeFromProjectAsync(Guid projectId);
}

