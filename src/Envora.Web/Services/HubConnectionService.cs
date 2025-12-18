using Envora.Web.Models.Notes;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Envora.Web.Services;

public sealed class HubConnectionService : IHubConnectionService, IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly EnvoraApiOptions _apiOptions;
    private readonly ILogger<HubConnectionService> _logger;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public event Action<NoteDto>? OnNoteAdded;
    public event Action<NoteDto>? OnNoteUpdated;
    public event Action<Guid>? OnNoteDeleted;
    public event Action<Guid, Guid, string>? OnReactionAdded;

    public HubConnectionService(
        IOptions<EnvoraApiOptions> apiOptions,
        ILogger<HubConnectionService> logger)
    {
        _apiOptions = apiOptions.Value;
        _logger = logger;
    }

    public async Task ConnectAsync()
    {
        if (IsConnected) return;

        var hubUrl = _apiOptions.ApiBaseUrl.TrimEnd('/') + "/hubs/project";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
            .Build();

        // Define server-to-client event handlers
        _hubConnection.On<NoteDto>("NoteAdded", note =>
        {
            _logger.LogDebug("SignalR: NoteAdded received for note {NoteId}", note.NoteId);
            OnNoteAdded?.Invoke(note);
        });

        _hubConnection.On<NoteDto>("NoteUpdated", note =>
        {
            _logger.LogDebug("SignalR: NoteUpdated received for note {NoteId}", note.NoteId);
            OnNoteUpdated?.Invoke(note);
        });

        _hubConnection.On<Guid>("NoteDeleted", noteId =>
        {
            _logger.LogDebug("SignalR: NoteDeleted received for note {NoteId}", noteId);
            OnNoteDeleted?.Invoke(noteId);
        });

        _hubConnection.On<Guid, Guid, string>("ReactionAdded", (noteId, userId, emoji) =>
        {
            _logger.LogDebug("SignalR: ReactionAdded received for note {NoteId}, emoji {Emoji}", noteId, emoji);
            OnReactionAdded?.Invoke(noteId, userId, emoji);
        });

        _hubConnection.Reconnecting += error =>
        {
            _logger.LogWarning(error, "SignalR: Reconnecting...");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += connectionId =>
        {
            _logger.LogInformation("SignalR: Reconnected with connection {ConnectionId}", connectionId);
            return Task.CompletedTask;
        };

        _hubConnection.Closed += error =>
        {
            _logger.LogWarning(error, "SignalR: Connection closed");
            return Task.CompletedTask;
        };

        try
        {
            await _hubConnection.StartAsync();
            _logger.LogInformation("SignalR: Connected to {HubUrl}", hubUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR: Connection failed to {HubUrl}", hubUrl);
            throw;
        }
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
            _logger.LogInformation("SignalR: Disconnected");
        }
    }

    public async Task SubscribeToProjectAsync(Guid projectId)
    {
        if (!IsConnected)
        {
            await ConnectAsync();
        }

        if (_hubConnection != null)
        {
            await _hubConnection.SendAsync("SubscribeToProject", projectId);
            _logger.LogDebug("SignalR: Subscribed to project {ProjectId}", projectId);
        }
    }

    public async Task UnsubscribeFromProjectAsync(Guid projectId)
    {
        if (_hubConnection != null && IsConnected)
        {
            await _hubConnection.SendAsync("UnsubscribeFromProject", projectId);
            _logger.LogDebug("SignalR: Unsubscribed from project {ProjectId}", projectId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
    }
}

