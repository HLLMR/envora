# Envora Platform - Blazor Component Architecture Guide

**Version:** 1.0  
**Date:** December 2025  
**Status:** Active Development  
**Audience:** Frontend Engineers, Full-Stack Developers  

---

## 1. Overview

This document defines Blazor component architecture, naming conventions, state management patterns, lifecycle integration, and real-time communication for the Envora Platform.

**Framework**: Blazor Server (ASP.NET Core 8.0)  
**UI Framework**: Bootstrap 5.3  
**Real-Time**: SignalR (WebSocket)  
**State**: Scoped/Transient services + cascading parameters  

---

## 2. Project Structure

```
src/
├── Envora.Web/                        # Blazor Server app
│   ├── Pages/                         # Route pages (PageRouteValue = page)
│   │   ├── Index.razor                # Dashboard
│   │   ├── Login.razor                # Login
│   │   ├── Projects/
│   │   │   ├── List.razor             # /projects
│   │   │   ├── Detail.razor           # /projects/{projectId}
│   │   │   ├── @layout.razor          # Layout for project pages
│   │   │   └── Disciplines/           # Nested folder for project disciplines
│   │   │       ├── OverviewPage.razor
│   │   │       ├── FinancialPage.razor
│   │   │       ├── SchedulePage.razor
│   │   │       ├── DesignPage.razor
│   │   │       └── ServicePage.razor
│   │   └── Admin/
│   │       ├── Dashboard.razor
│   │       └── Settings.razor
│   │
│   ├── Components/                    # Reusable components (no @page)
│   │   ├── Shared/
│   │   │   ├── MainLayout.razor       # Root layout
│   │   │   ├── SidebarNav.razor       # Discipline sidebar
│   │   │   ├── Header.razor           # Top navigation
│   │   │   └── ProjectSwitcher.razor  # Project dropdown
│   │   │
│   │   ├── Discipline/                # Discipline-specific components
│   │   │   ├── DisciplineNav.razor    # Tab navigation for discipline
│   │   │   ├── OverviewTabs/
│   │   │   │   ├── SummaryTab.razor
│   │   │   │   ├── TeamTab.razor
│   │   │   │   ├── DocumentsTab.razor
│   │   │   │   ├── ActivityTab.razor
│   │   │   │   ├── NotesTab.razor
│   │   │   │   └── SettingsTab.razor
│   │   │   ├── DesignTabs/
│   │   │   │   ├── EquipmentTab.razor
│   │   │   │   ├── PointsTab.razor
│   │   │   │   ├── SchedulesTab.razor
│   │   │   │   ├── DrawingsTab.razor
│   │   │   │   └── SequencesTab.razor
│   │   │   └── [other disciplines...]
│   │   │
│   │   ├── Notes/                     # Persistent notes components
│   │   │   ├── NotesPanelComponent.razor   # Bottom panel (every tab)
│   │   │   ├── CommentThreadComponent.razor
│   │   │   ├── ReactionComponent.razor
│   │   │   └── MentionInputComponent.razor
│   │   │
│   │   ├── Equipment/                 # Equipment components
│   │   │   ├── EquipmentListComponent.razor
│   │   │   ├── EquipmentDetailComponent.razor
│   │   │   ├── EquipmentFormComponent.razor
│   │   │   ├── EquipmentImportComponent.razor
│   │   │   └── QuickDrilldownComponent.razor
│   │   │
│   │   ├── Common/                    # Generic/reusable
│   │   │   ├── LoadingSpinnerComponent.razor
│   │   │   ├── ModalComponent.razor
│   │   │   ├── ConfirmDialogComponent.razor
│   │   │   ├── PaginationComponent.razor
│   │   │   ├── SearchBarComponent.razor
│   │   │   ├── FilterComponent.razor
│   │   │   ├── TabsComponent.razor
│   │   │   └── DataTableComponent.razor
│   │   │
│   │   └── Submittal/                 # Submittal workflow
│   │       ├── SubmittalWizardComponent.razor
│   │       ├── SubmittalStatusComponent.razor
│   │       └── SubmittalHistoryComponent.razor
│   │
│   ├── Services/                      # Business logic & API integration
│   │   ├── Interfaces/
│   │   │   ├── IProjectService.cs
│   │   │   ├── IEquipmentService.cs
│   │   │   ├── IPointService.cs
│   │   │   ├── INotesService.cs
│   │   │   ├── ISubmittalService.cs
│   │   │   ├── IAuthService.cs
│   │   │   └── IHubConnectionService.cs (SignalR)
│   │   │
│   │   ├── Implementations/
│   │   │   ├── ProjectService.cs
│   │   │   ├── EquipmentService.cs
│   │   │   ├── PointService.cs
│   │   │   ├── NotesService.cs
│   │   │   ├── SubmittalService.cs
│   │   │   ├── AuthService.cs
│   │   │   └── HubConnectionService.cs
│   │   │
│   │   └── HttpClientInterceptor.cs   # Request/response interceptor
│   │
│   ├── Models/                        # DTO models (mirror API contracts)
│   │   ├── Project/
│   │   │   ├── ProjectListDto.cs
│   │   │   ├── ProjectDetailDto.cs
│   │   │   ├── CreateProjectDto.cs
│   │   │   └── UpdateProjectDto.cs
│   │   ├── Equipment/
│   │   │   ├── EquipmentDto.cs
│   │   │   ├── CreateEquipmentDto.cs
│   │   │   └── EquipmentImportDto.cs
│   │   ├── Notes/
│   │   │   ├── NoteDto.cs
│   │   │   ├── CreateNoteDto.cs
│   │   │   └── ReactionDto.cs
│   │   ├── Auth/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── LoginResponse.cs
│   │   │   └── UserDto.cs
│   │   └── Shared/
│   │       ├── ApiResponse.cs
│   │       ├── PaginatedResponse.cs
│   │       └── ErrorResponse.cs
│   │
│   ├── Utils/                         # Utilities
│   │   ├── DateTimeHelper.cs
│   │   ├── StringHelper.cs
│   │   ├── ValidationHelper.cs
│   │   └── MentionParser.cs           # Parse @mentions in notes
│   │
│   ├── wwwroot/                       # Static assets
│   │   ├── css/
│   │   │   ├── design-system.css      # Color variables, typography
│   │   │   ├── app.css                # Global styles
│   │   │   ├── components/
│   │   │   │   ├── sidebar.css
│   │   │   │   ├── notes-panel.css
│   │   │   │   ├── modal.css
│   │   │   │   └── [component styles]
│   │   │   └── responsive.css         # Mobile/tablet breakpoints
│   │   ├── js/
│   │   │   ├── interop.js             # JS interop functions
│   │   │   ├── notifications.js
│   │   │   └── keyboard-shortcuts.js  # CMD+K, etc.
│   │   └── images/
│   │       ├── icons/
│   │       └── logos/
│   │
│   ├── App.razor                      # Root component
│   ├── Program.cs                     # Dependency injection & middleware setup
│   └── appsettings.json               # Config (API base URL, auth, etc.)
│
├── Envora.Api/                        # ASP.NET Core API
│   ├── Controllers/
│   ├── Services/
│   ├── Data/
│   └── [...]
│
└── Envora.Tests/
    ├── ComponentTests/                # Blazor component tests (bUnit)
    ├── ServiceTests/                  # Service unit tests
    └── IntegrationTests/              # End-to-end tests
```

---

## 3. Page vs. Component Distinction

### Pages (@page directive)

Pages are **routable components** with `@page` directive. They represent **full-screen views**.

**Naming**: `[Feature]Page.razor` or `[Feature].razor`

```razor
@page "/projects/{ProjectId:int}"
@using Envora.Web.Components
@using Envora.Web.Services
@using Envora.Web.Models

@layout ProjectDetailLayout

@inject IProjectService ProjectService
@inject NavigationManager NavManager
@inject IJSRuntime JS

<DisciplineNav CurrentDiscipline="@CurrentDiscipline" ProjectId="@ProjectId" />

<div class="tab-content">
    @switch(CurrentTab)
    {
        case "Summary":
            <SummaryTab Project="@Project" />
            break;
        case "Team":
            <TeamTab Project="@Project" />
            break;
        case "Documents":
            <DocumentsTab Project="@Project" />
            break;
    }
</div>

<NotesPanelComponent ProjectId="@ProjectId" Context="@ContextString" />

@code {
    [Parameter] public int ProjectId { get; set; }
    [CascadingParameter] public string CurrentDiscipline { get; set; }
    private string CurrentTab { get; set; } = "Summary";
    private ProjectDetailDto Project { get; set; }
    private string ContextString => $"{CurrentDiscipline}:{CurrentTab}";

    protected override async Task OnInitializedAsync()
    {
        Project = await ProjectService.GetProjectAsync(ProjectId);
    }
}
```

---

### Components (no @page directive)

Components are **reusable, non-routable UI blocks**. They accept parameters and emit events.

**Naming**: `[Feature]Component.razor` or `[Feature]Control.razor`

```razor
@using Envora.Web.Models
@using Envora.Web.Services

<div class="equipment-list">
    @if (Equipment == null)
    {
        <LoadingSpinnerComponent />
    }
    else if (Equipment.Count == 0)
    {
        <div class="alert alert-info">No equipment found. Create one?</div>
    }
    else
    {
        <table class="table table-striped">
            @foreach (var eq in Equipment)
            {
                <tr @onclick="() => OnEquipmentSelected.InvokeAsync(eq)">
                    <td>@eq.EquipmentTag</td>
                    <td>@eq.EquipmentType</td>
                    <td>@eq.Manufacturer</td>
                </tr>
            }
        </table>
    }
</div>

@code {
    [Parameter] public List<EquipmentDto> Equipment { get; set; } = new();
    [Parameter] public EventCallback<EquipmentDto> OnEquipmentSelected { get; set; }
}
```

---

## 4. State Management Pattern

### Pattern 1: Scoped Service (Page-Level State)

**Use when**: A single page needs to manage state (e.g., current project, selected tab, filters).

```csharp
// Services/ProjectStateService.cs
public interface IProjectStateService
{
    int CurrentProjectId { get; set; }
    string CurrentDiscipline { get; set; }
    string CurrentTab { get; set; }
    event Action OnStateChanged;
    void NotifyStateChanged();
}

public class ProjectStateService : IProjectStateService
{
    public int CurrentProjectId { get; set; }
    public string CurrentDiscipline { get; set; }
    public string CurrentTab { get; set; }
    
    public event Action OnStateChanged;
    
    public void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}

// Program.cs
services.AddScoped<IProjectStateService, ProjectStateService>();
```

**Usage in component**:

```razor
@page "/projects/{ProjectId:int}/design/equipment"
@implements IAsyncDisposable
@inject IProjectStateService ProjectState

<EquipmentListComponent Equipment="@Equipment" />

@code {
    [Parameter] public int ProjectId { get; set; }
    private List<EquipmentDto> Equipment { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ProjectState.CurrentProjectId = ProjectId;
        ProjectState.CurrentDiscipline = "Design";
        ProjectState.CurrentTab = "Equipment";
        ProjectState.OnStateChanged += StateHasChanged;
        
        Equipment = await FetchEquipmentAsync();
        ProjectState.NotifyStateChanged();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        ProjectState.OnStateChanged -= StateHasChanged;
    }
}
```

---

### Pattern 2: Cascading Parameters (Parent-to-Child Communication)

**Use when**: Parent passes data down to multiple child components.

```razor
@* Parent: OverviewPage.razor *@
<CascadingValue Value="@Project">
    <SummaryTab />
    <TeamTab />
    <DocumentsTab />
</CascadingValue>

@code {
    private ProjectDetailDto Project { get; set; }
}

@* Child: SummaryTab.razor *@
@code {
    [CascadingParameter] public ProjectDetailDto Project { get; set; }
}
```

---

### Pattern 3: Component Events (Child-to-Parent Communication)

**Use when**: Child needs to notify parent of state changes.

```razor
@* Parent: EquipmentTabPage.razor *@
<EquipmentListComponent Equipment="@Equipment" OnEquipmentSelected="HandleEquipmentSelected" />
<EquipmentDetailComponent Equipment="@SelectedEquipment" />

@code {
    private List<EquipmentDto> Equipment { get; set; }
    private EquipmentDto SelectedEquipment { get; set; }

    private async Task HandleEquipmentSelected(EquipmentDto equipment)
    {
        SelectedEquipment = equipment;
        await InvokeAsync(StateHasChanged);
    }
}

@* Child: EquipmentListComponent.razor *@
[Parameter] public EventCallback<EquipmentDto> OnEquipmentSelected { get; set; }

private async Task SelectEquipment(EquipmentDto eq)
{
    await OnEquipmentSelected.InvokeAsync(eq);
}
```

---

## 5. Lifecycle & Initialization Pattern

All pages should follow this lifecycle pattern:

```razor
@page "/projects/{ProjectId:int}"
@implements IAsyncDisposable
@inject IProjectService ProjectService
@inject IHubConnectionService HubService

@if (IsLoading)
{
    <LoadingSpinnerComponent />
}
else if (Error != null)
{
    <ErrorAlertComponent Message="@Error" OnDismiss="HandleErrorDismiss" />
}
else
{
    <!-- Content here -->
}

@code {
    [Parameter] public int ProjectId { get; set; }
    
    private ProjectDetailDto Project { get; set; }
    private bool IsLoading { get; set; } = true;
    private string Error { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // 1. Fetch data
            Project = await ProjectService.GetProjectAsync(ProjectId);
            
            // 2. Subscribe to real-time updates (if applicable)
            HubService.OnNoteAdded += HandleNoteAdded;
            await HubService.JoinProjectGroupAsync(ProjectId);
        }
        catch (HttpRequestException ex)
        {
            Error = $"Failed to load project: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        // If parameters change (e.g., navigating between projects), reload
        if (Project?.ProjectId != ProjectId)
        {
            IsLoading = true;
            await OnInitializedAsync();
        }
    }

    private void HandleNoteAdded(NoteDto note)
    {
        // Update UI when real-time event received
        InvokeAsync(StateHasChanged);
    }

    private void HandleErrorDismiss()
    {
        Error = null;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        HubService.OnNoteAdded -= HandleNoteAdded;
        if (HubService.IsConnected)
        {
            await HubService.LeaveProjectGroupAsync(ProjectId);
        }
    }
}
```

---

## 6. SignalR Integration Pattern

### Setup (Program.cs)

```csharp
// Program.cs
var builder = WebAssemblyBuilder.CreateDefault(args);

// Add HttpClient
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

// Add SignalR
builder.Services.AddScoped<IHubConnectionService, HubConnectionService>();

// Add services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<INotesService, NotesService>();

await builder.Build().RunAsync();
```

### HubConnectionService Interface

```csharp
public interface IHubConnectionService
{
    bool IsConnected { get; }
    
    // Events
    event Action<NoteDto> OnNoteAdded;
    event Action<NoteDto> OnNoteUpdated;
    event Action<int> OnNoteDeleted;
    event Action<int, ReactionDto> OnReactionAdded;
    event Action<JobStatusUpdate> OnJobStatusUpdated;
    event Action<UserStatusUpdate> OnUserStatusChanged;
    
    // Methods
    Task ConnectAsync();
    Task DisconnectAsync();
    Task JoinProjectGroupAsync(int projectId);
    Task LeaveProjectGroupAsync(int projectId);
    Task AddNoteAsync(int projectId, CreateNoteDto note);
    Task UpdateNoteAsync(int projectId, int noteId, string content);
    Task DeleteNoteAsync(int projectId, int noteId);
}
```

### Implementation

```csharp
public class HubConnectionService : IHubConnectionService
{
    private HubConnection _hubConnection;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HubConnectionService> _logger;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public event Action<NoteDto> OnNoteAdded;
    public event Action<NoteDto> OnNoteUpdated;
    public event Action<int> OnNoteDeleted;
    public event Action<int, ReactionDto> OnReactionAdded;
    public event Action<JobStatusUpdate> OnJobStatusUpdated;
    public event Action<UserStatusUpdate> OnUserStatusChanged;

    public HubConnectionService(HttpClient httpClient, ILogger<HubConnectionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task ConnectAsync()
    {
        if (IsConnected) return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}hubs/project")
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10) })
            .Build();

        // Define server-to-client event handlers
        _hubConnection.On<NoteDto>("NoteAdded", note => OnNoteAdded?.Invoke(note));
        _hubConnection.On<NoteDto>("NoteUpdated", note => OnNoteUpdated?.Invoke(note));
        _hubConnection.On<int>("NoteDeleted", noteId => OnNoteDeleted?.Invoke(noteId));
        _hubConnection.On<int, ReactionDto>("ReactionAdded", (noteId, reaction) => 
            OnReactionAdded?.Invoke(noteId, reaction));
        _hubConnection.On<JobStatusUpdate>("JobStatusUpdated", update => 
            OnJobStatusUpdated?.Invoke(update));
        _hubConnection.On<UserStatusUpdate>("UserStatusChanged", update => 
            OnUserStatusChanged?.Invoke(update));

        try
        {
            await _hubConnection.StartAsync();
            _logger.LogInformation("SignalR connected");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR connection failed");
            throw;
        }
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }

    public async Task JoinProjectGroupAsync(int projectId)
    {
        if (!IsConnected) await ConnectAsync();
        await _hubConnection.SendAsync("JoinProjectGroup", projectId);
    }

    public async Task LeaveProjectGroupAsync(int projectId)
    {
        if (IsConnected)
        {
            await _hubConnection.SendAsync("LeaveProjectGroup", projectId);
        }
    }

    public async Task AddNoteAsync(int projectId, CreateNoteDto note)
    {
        if (!IsConnected) await ConnectAsync();
        await _hubConnection.SendAsync("AddNote", projectId, note);
    }

    public async Task UpdateNoteAsync(int projectId, int noteId, string content)
    {
        if (!IsConnected) await ConnectAsync();
        await _hubConnection.SendAsync("UpdateNote", projectId, noteId, content);
    }

    public async Task DeleteNoteAsync(int projectId, int noteId)
    {
        if (!IsConnected) await ConnectAsync();
        await _hubConnection.SendAsync("DeleteNote", projectId, noteId);
    }
}
```

### Usage in Component

```razor
@page "/projects/{ProjectId:int}/overview/summary"
@implements IAsyncDisposable
@inject IHubConnectionService HubService
@inject INotesService NotesService

<div>
    <h2>Project Summary</h2>
    <!-- Content -->
</div>

<NotesPanelComponent ProjectId="@ProjectId" Context="OVERVIEW:Summary" />

@code {
    [Parameter] public int ProjectId { get; set; }
    private List<NoteDto> Notes { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        // Subscribe to real-time note events
        HubService.OnNoteAdded += HandleNoteAdded;
        HubService.OnNoteUpdated += HandleNoteUpdated;
        HubService.OnNoteDeleted += HandleNoteDeleted;

        // Join project group
        await HubService.JoinProjectGroupAsync(ProjectId);

        // Load initial notes
        Notes = await NotesService.GetProjectNotesAsync(ProjectId, "OVERVIEW:Summary");
    }

    private void HandleNoteAdded(NoteDto note)
    {
        if (note.ContentContext == "OVERVIEW:Summary")
        {
            Notes.Add(note);
            InvokeAsync(StateHasChanged);
        }
    }

    private void HandleNoteUpdated(NoteDto note)
    {
        var existing = Notes.FirstOrDefault(n => n.NoteId == note.NoteId);
        if (existing != null)
        {
            existing.Content = note.Content;
            existing.EditedAt = note.EditedAt;
            InvokeAsync(StateHasChanged);
        }
    }

    private void HandleNoteDeleted(int noteId)
    {
        Notes.RemoveAll(n => n.NoteId == noteId);
        InvokeAsync(StateHasChanged);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        HubService.OnNoteAdded -= HandleNoteAdded;
        HubService.OnNoteUpdated -= HandleNoteUpdated;
        HubService.OnNoteDeleted -= HandleNoteDeleted;
        await HubService.LeaveProjectGroupAsync(ProjectId);
    }
}
```

---

## 7. Form Validation Pattern

### Client-Side + Server-Side

```razor
@* EquipmentFormComponent.razor *@
@using System.ComponentModel.DataAnnotations

<EditForm Model="@Equipment" OnValidSubmit="HandleSubmit" OnInvalidSubmit="HandleInvalidSubmit">
    <DataAnnotationsValidator />
    
    <div class="form-group">
        <label for="tag">Equipment Tag</label>
        <InputText id="tag" class="form-control" @bind-Value="Equipment.EquipmentTag" />
        <ValidationMessage For="() => Equipment.EquipmentTag" />
    </div>

    <div class="form-group">
        <label for="type">Type</label>
        <InputSelect id="type" class="form-control" @bind-Value="Equipment.EquipmentType">
            <option value="">-- Select Type --</option>
            <option value="RTU">RTU</option>
            <option value="AHU">AHU</option>
            <option value="VAV">VAV</option>
        </InputSelect>
        <ValidationMessage For="() => Equipment.EquipmentType" />
    </div>

    <button type="submit" class="btn btn-primary">Save</button>
</EditForm>

@code {
    [Parameter] public EventCallback OnSaved { get; set; }
    private CreateEquipmentDto Equipment { get; set; } = new();
    private string ErrorMessage { get; set; }

    private async Task HandleSubmit()
    {
        try
        {
            await EquipmentService.CreateEquipmentAsync(Equipment);
            await OnSaved.InvokeAsync();
        }
        catch (ValidationException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private void HandleInvalidSubmit()
    {
        ErrorMessage = "Please check the form for errors.";
    }
}

public class CreateEquipmentDto
{
    [Required(ErrorMessage = "Equipment tag is required")]
    [StringLength(50, ErrorMessage = "Tag must be 50 characters or less")]
    public string EquipmentTag { get; set; }

    [Required(ErrorMessage = "Equipment type is required")]
    public string EquipmentType { get; set; }

    [MaxLength(500, ErrorMessage = "Comments must be 500 characters or less")]
    public string Comments { get; set; }
}
```

---

## 8. Service Pattern (API Integration)

### ProjectService Example

```csharp
public interface IProjectService
{
    Task<List<ProjectListDto>> GetProjectsAsync(int skip = 0, int take = 20);
    Task<ProjectDetailDto> GetProjectAsync(int projectId);
    Task<ProjectDetailDto> CreateProjectAsync(CreateProjectDto project);
    Task<ProjectDetailDto> UpdateProjectAsync(int projectId, UpdateProjectDto project);
    Task DeleteProjectAsync(int projectId);
}

public class ProjectService : IProjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(HttpClient httpClient, ILogger<ProjectService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ProjectListDto>> GetProjectsAsync(int skip = 0, int take = 20)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/v1/projects?skip={skip}&take={take}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<ProjectListDto>>>(json);
            return result?.Data ?? new();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch projects");
            throw;
        }
    }

    public async Task<ProjectDetailDto> GetProjectAsync(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/v1/projects/{projectId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<ProjectDetailDto>>(json);
            return result?.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Failed to fetch project {projectId}");
            throw;
        }
    }

    public async Task<ProjectDetailDto> CreateProjectAsync(CreateProjectDto project)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(project),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("/api/v1/projects", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<ProjectDetailDto>>(json);
            return result?.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to create project");
            throw;
        }
    }

    public async Task<ProjectDetailDto> UpdateProjectAsync(int projectId, UpdateProjectDto project)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(project),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PatchAsync($"/api/v1/projects/{projectId}", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<ProjectDetailDto>>(json);
            return result?.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Failed to update project {projectId}");
            throw;
        }
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/projects/{projectId}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Failed to delete project {projectId}");
            throw;
        }
    }
}
```

---

## 9. Error Handling Pattern

### Global Error Boundary

```razor
@* App.razor *@
<CascadingValue Value="this">
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>
        <NotFound>
            <PageTitle>Not found</PageTitle>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>
</CascadingValue>

@code {
    [CascadingParameter] public App App { get; set; }
}
```

### Component Error Handling

```razor
@if (Error != null)
{
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        <strong>Error:</strong> @Error
        <button type="button" class="btn-close" @onclick="() => Error = null" />
    </div>
}

@code {
    private string Error { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Logic here
        }
        catch (HttpRequestException ex)
        {
            Error = $"Network error: {ex.Message}";
        }
        catch (Exception ex)
        {
            Error = $"Unexpected error: {ex.Message}";
            // Log to Application Insights
        }
    }
}
```

---

## 10. Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| **Page** | `[Feature]Page.razor` | `ProjectDetailPage.razor`, `OverviewPage.razor` |
| **Component** | `[Feature]Component.razor` | `EquipmentListComponent.razor`, `NotesPanelComponent.razor` |
| **Service Interface** | `I[Feature]Service.cs` | `IProjectService.cs`, `INotesService.cs` |
| **Service Implementation** | `[Feature]Service.cs` | `ProjectService.cs` |
| **Model/DTO** | `[Feature]Dto.cs` or `[Feature]Request.cs` | `ProjectDetailDto.cs`, `CreateEquipmentDto.cs` |
| **Enum** | `[Feature]Status` | `ProjectStatus`, `EquipmentType` |
| **Parameter** | `[CascadingParameter]` or `[Parameter]` | `Project`, `ProjectId` |
| **Event** | `On[Action]` | `OnEquipmentSelected`, `OnSaved` |
| **Method** | `Handle[Action]`, `On[Action]` | `HandleSubmit()`, `OnInitializedAsync()` |
| **Variable** | `camelCase` | `equipment`, `errorMessage` |

---

## 11. Best Practices

### Do ✅
- ✅ Use `async/await` exclusively for I/O operations
- ✅ Call `InvokeAsync(StateHasChanged)` when updating state from events
- ✅ Implement `IAsyncDisposable` to clean up subscriptions
- ✅ Use cascading parameters for parent-to-child data
- ✅ Use events/callbacks for child-to-parent communication
- ✅ Keep components small and focused (single responsibility)
- ✅ Validate both client-side and server-side
- ✅ Use services for all API calls
- ✅ Subscribe to real-time events in `OnInitializedAsync`
- ✅ Unsubscribe in `DisposeAsync`

### Don't ❌
- ❌ Don't make multiple rapid API calls in a loop (batch instead)
- ❌ Don't call `StateHasChanged()` directly from event handlers (use `InvokeAsync`)
- ❌ Don't embed API calls in components (use services)
- ❌ Don't forget to unsubscribe from events
- ❌ Don't mix presentation logic with business logic
- ❌ Don't make components too large (>300 lines)
- ❌ Don't use two-way binding excessively
- ❌ Don't ignore disposal/cleanup

---

## 12. Testing Components (bUnit)

```csharp
[TestClass]
public class EquipmentListComponentTests
{
    private TestContext _context;

    [TestInitialize]
    public void Setup()
    {
        _context = new TestContext();
    }

    [TestMethod]
    public void RendersEquipmentTable()
    {
        var equipment = new List<EquipmentDto>
        {
            new() { EquipmentId = 1, EquipmentTag = "RTU-1", EquipmentType = "RTU" }
        };

        var cut = _context.RenderComponent<EquipmentListComponent>(
            parameters => parameters.Add(p => p.Equipment, equipment)
        );

        cut.Find("table").Should().NotBeNull();
        cut.Find("tbody tr").TextContent.Should().Contain("RTU-1");
    }

    [TestMethod]
    public async Task EmitsEventOnEquipmentSelected()
    {
        var equipment = new List<EquipmentDto>
        {
            new() { EquipmentId = 1, EquipmentTag = "RTU-1", EquipmentType = "RTU" }
        };

        var selectedEquipment = (EquipmentDto)null;

        var cut = _context.RenderComponent<EquipmentListComponent>(
            parameters => parameters
                .Add(p => p.Equipment, equipment)
                .Add(p => p.OnEquipmentSelected, EventCallback.Factory.Create<EquipmentDto>(
                    this, eq => selectedEquipment = eq
                ))
        );

        await cut.Find("tbody tr").ClickAsync(new MouseEventArgs());

        selectedEquipment.Should().NotBeNull();
        selectedEquipment.EquipmentTag.Should().Be("RTU-1");
    }
}
```

---

## 13. Document Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | Dec 18, 2025 | Frontend Team | Initial Blazor Architecture Guide |

