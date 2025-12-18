# Envora Platform - Development Workflow & Coding Standards

**Version:** 1.0  
**Date:** December 2025  
**Status:** Active Development  
**Audience:** Engineering Team, DevOps, QA  

---

## 1. Overview

This document establishes development practices, coding standards, testing requirements, and deployment workflows for the Envora Platform.

**Tech Stack**:
- **.NET**: 8.0 (LTS)
- **Frontend**: Blazor Server, Bootstrap 5.3
- **Database**: Azure SQL Database
- **Storage**: Azure Blob Storage
- **Messaging**: Azure Service Bus
- **Monitoring**: Application Insights
- **Testing**: xUnit, Moq, bUnit
- **CI/CD**: GitHub Actions or Azure DevOps
- **VCS**: Git (GitHub)

---

## 2. Repository Structure

```
envora-platform/
├── .github/
│   ├── workflows/
│   │   ├── build.yml                  # Build pipeline
│   │   ├── test.yml                   # Test pipeline
│   │   └── deploy.yml                 # Deploy to Azure
│   └── pull_request_template.md       # PR template
│
├── src/
│   ├── Envora.Api/                    # ASP.NET Core 8.0 API
│   │   ├── Controllers/
│   │   │   ├── ProjectsController.cs
│   │   │   ├── EquipmentController.cs
│   │   │   ├── PointsController.cs
│   │   │   ├── NotesController.cs
│   │   │   ├── SubmittalController.cs
│   │   │   ├── DocumentsController.cs
│   │   │   ├── AuthController.cs
│   │   │   └── AdminController.cs
│   │   │
│   │   ├── Services/
│   │   │   ├── Interfaces/
│   │   │   ├── Implementations/
│   │   │   └── BackgroundServices/
│   │   │
│   │   ├── Data/
│   │   │   ├── EnvoraDbContext.cs
│   │   │   ├── Entities/
│   │   │   │   ├── Project.cs
│   │   │   │   ├── Equipment.cs
│   │   │   │   ├── Point.cs
│   │   │   │   ├── Note.cs
│   │   │   │   ├── User.cs
│   │   │   │   ├── Company.cs
│   │   │   │   └── [other entities]
│   │   │   ├── Configurations/
│   │   │   └── Migrations/
│   │   │
│   │   ├── Hubs/
│   │   │   └── ProjectHub.cs           # SignalR hub for real-time
│   │   │
│   │   ├── Models/
│   │   │   ├── Dtos/
│   │   │   │   ├── ProjectDto.cs
│   │   │   │   ├── EquipmentDto.cs
│   │   │   │   └── [other DTOs]
│   │   │   ├── Requests/
│   │   │   ├── Responses/
│   │   │   └── Shared/
│   │   │
│   │   ├── Middleware/
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   ├── RequestLoggingMiddleware.cs
│   │   │   └── JwtAuthenticationMiddleware.cs
│   │   │
│   │   ├── Jobs/
│   │   │   ├── VisioExportJob.cs      # Visio diagram generation
│   │   │   ├── SubmittalGenerationJob.cs
│   │   │   └── PdfExportJob.cs
│   │   │
│   │   ├── Utils/
│   │   │   ├── ValidationHelper.cs
│   │   │   ├── MappingProfile.cs      # AutoMapper
│   │   │   └── Constants.cs
│   │   │
│   │   ├── Program.cs                 # Dependency injection setup
│   │   ├── appsettings.json           # Config
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Production.json
│   │   └── Envora.Api.csproj
│   │
│   ├── Envora.Web/                    # Blazor Server app
│   │   ├── Pages/
│   │   ├── Components/
│   │   ├── Services/
│   │   ├── Models/
│   │   ├── wwwroot/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── Envora.Web.csproj
│   │
│   ├── Envora.Domain/                 # Domain models (shared)
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   └── Envora.Domain.csproj
│   │
│   └── Envora.sln                     # Solution file
│
├── tests/
│   ├── Envora.Api.Tests/              # API unit tests
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Fixtures/
│   │   └── Envora.Api.Tests.csproj
│   │
│   ├── Envora.Web.Tests/              # Blazor component tests (bUnit)
│   │   ├── Components/
│   │   ├── Services/
│   │   └── Envora.Web.Tests.csproj
│   │
│   └── Envora.Integration.Tests/      # End-to-end tests
│       ├── ApiTests/
│       ├── Fixtures/
│       └── Envora.Integration.Tests.csproj
│
├── docs/
│   ├── API_SPECIFICATION.md           # REST API spec
│   ├── BLAZOR_ARCHITECTURE.md         # Component architecture
│   ├── DATABASE_SCHEMA.md             # DDL & relationships
│   ├── DEPLOYMENT_GUIDE.md            # Azure deployment
│   └── TROUBLESHOOTING.md
│
├── .gitignore
├── README.md
├── CONTRIBUTING.md
├── LICENSE
└── docker-compose.yml                 # Local dev environment

```

---

## 3. Git Workflow

### Branch Strategy: GitHub Flow

```
main (production)
  ↓
develop (integration)
  ↓
feature/* (feature branches)
  ↓
hotfix/* (emergency fixes)
```

### Naming Conventions

**Feature branches**: `feature/[feature-name]`
- `feature/project-crud`
- `feature/equipment-import`
- `feature/persistent-notes`

**Bug branches**: `bugfix/[bug-name]`
- `bugfix/notes-not-saving`
- `bugfix/equipment-pagination`

**Hotfix branches**: `hotfix/[issue-name]`
- `hotfix/api-500-error`

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>

Types: feat, fix, docs, style, refactor, test, chore, ci
Scopes: api, web, db, auth, equipment, notes, submittal
```

**Examples**:
```
feat(equipment): add bulk import endpoint

- Implement POST /projects/{id}/equipment/import
- Add validation for equipment list
- Auto-generate points from templates
- Add audit logging

Closes #123
```

```
fix(notes): prevent duplicate mentions notifications

- Filter duplicate mentions before sending
- Add unit tests

Fixes #456
```

### Pull Request Process

1. **Create PR from feature branch → develop**
   - Link to issue (Closes #123)
   - Add description of changes
   - Add before/after screenshots (if UI change)

2. **Code Review** (minimum 1 approval)
   - At least one team member review
   - Address feedback before merge

3. **CI Pipeline** (automated)
   - Build succeeds
   - All tests pass (>80% coverage)
   - Linting passes
   - No security vulnerabilities

4. **Merge to develop**
   - Use "Squash and merge" for clean history
   - Delete branch after merge

5. **Deploy to develop environment**
   - Automated via CI/CD
   - Smoke tests run

6. **Promote to main**
   - After QA sign-off on develop
   - Create release PR (main ← develop)
   - Auto-deploy to production

---

## 4. C# Coding Standards

### Naming Conventions

```csharp
// Classes, Interfaces, Methods: PascalCase
public class ProjectService { }
public interface IProjectService { }
public void CreateProject() { }

// Properties: PascalCase
public string ProjectName { get; set; }

// Private fields: _camelCase
private readonly IProjectService _projectService;
private string _errorMessage;

// Local variables, parameters: camelCase
var projectId = 1;
decimal totalAmount = 1500.00m;
```

### Class Organization

```csharp
public class ProjectService : IProjectService
{
    // 1. Fields & Constants (private first)
    private readonly ILogger<ProjectService> _logger;
    private readonly IRepository<Project> _repository;
    private const string LogPrefix = "ProjectService";

    // 2. Constructor
    public ProjectService(
        ILogger<ProjectService> logger,
        IRepository<Project> repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    // 3. Public methods (sorted: Get, List, Create, Update, Delete)
    public async Task<ProjectDto> GetProjectAsync(int projectId)
    {
        // Implementation
    }

    public async Task<List<ProjectDto>> GetProjectsAsync(int skip, int take)
    {
        // Implementation
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
    {
        // Implementation
    }

    // 4. Private methods
    private void ValidateProject(Project project)
    {
        // Validation logic
    }
}
```

### Use Modern C# Features

✅ **DO**:
```csharp
// Records for DTOs
public record CreateProjectDto(
    string ProjectNumber,
    string JobName,
    decimal ContractAmount);

// Nullable reference types
public string? OptionalField { get; set; }

// Pattern matching
if (project is { Status: ProjectStatus.Active })
{
    // Process active project
}

// Expression-bodied members
public bool IsOverdue => _dueDate < DateTime.Today;

// Async/await
public async Task<ProjectDto> GetAsync(int id)
{
    return await _repository.FindAsync(id);
}

// Linq
var activeProjects = projects
    .Where(p => p.Status == ProjectStatus.Active)
    .OrderBy(p => p.CreatedDate)
    .ToList();
```

❌ **DON'T**:
```csharp
// LINQ methods chained poorly
projects.Where(p => p.Status == "Active").Select(p => p.Name).First();

// Deep nesting
if (project != null)
{
    if (project.Team != null)
    {
        if (project.Team.Members != null)
        {
            // 3 levels deep
        }
    }
}

// Bare exceptions
catch (Exception) { }

// Magic numbers
if (project.Progress > 75) { }  // What does 75 mean?
```

### Async/Await Rules

```csharp
// ✅ DO: Use async/await for I/O operations
public async Task<ProjectDto> GetProjectAsync(int projectId)
{
    var project = await _repository.FindAsync(projectId);
    return _mapper.Map<ProjectDto>(project);
}

// ✅ DO: Use CancellationToken
public async Task<ProjectDto> GetProjectAsync(
    int projectId,
    CancellationToken cancellationToken = default)
{
    return await _repository.FindAsync(projectId, cancellationToken);
}

// ✅ DO: ConfigureAwait(false) in libraries
var project = await _repository.FindAsync(projectId).ConfigureAwait(false);

// ❌ DON'T: Fire and forget
_ = someTask;  // Only if intentional (log it!)

// ❌ DON'T: Sync over async
var result = GetAsync(1).Result;  // Causes deadlocks!
```

---

## 5. Entity Framework Best Practices

### DbContext Configuration

```csharp
public class EnvoraDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Point> Points { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entities
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
        modelBuilder.ApplyConfiguration(new NoteConfiguration());

        // Add seed data
        modelBuilder.Entity<ProjectStatus>().HasData(
            new { Id = 1, Name = "Draft" },
            new { Id = 2, Name = "InProgress" }
        );
    }
}
```

### Entity Configuration

```csharp
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.ProjectId);

        builder.Property(p => p.ProjectNumber)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(p => p.ContractAmount)
            .HasPrecision(18, 2);

        builder.HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Equipment)
            .WithOne(e => e.Project)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.ProjectNumber).IsUnique();
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedDate);
    }
}
```

### Queries (Use DbSet extensions)

```csharp
// ✅ DO: Filter at database level
var activeProjects = await _context.Projects
    .Where(p => p.Status == ProjectStatus.Active)
    .OrderByDescending(p => p.CreatedDate)
    .Skip(skip)
    .Take(take)
    .ToListAsync();

// ✅ DO: Use Include for related data
var project = await _context.Projects
    .Include(p => p.Customer)
    .Include(p => p.Equipment).ThenInclude(e => e.Points)
    .FirstOrDefaultAsync(p => p.ProjectId == id);

// ✅ DO: Use .AsNoTracking() for read-only queries
var projectList = await _context.Projects
    .AsNoTracking()
    .Where(p => p.Status == ProjectStatus.Active)
    .ToListAsync();

// ❌ DON'T: Load all then filter (N+1 problem)
var projects = _context.Projects.ToList();
var active = projects.Where(p => p.Status == ProjectStatus.Active).ToList();

// ❌ DON'T: Multiple round trips
var project = _context.Projects.Find(id);  // Query 1
var equipment = _context.Equipment.Where(e => e.ProjectId == id).ToList();  // Query 2
```

---

## 6. Logging & Monitoring

### Structured Logging

```csharp
// ✅ DO: Use structured logging
_logger.LogInformation(
    "Project {ProjectId} created by {UserId} with contract amount {ContractAmount}",
    project.ProjectId,
    userId,
    project.ContractAmount);

// Application Insights custom event
var properties = new Dictionary<string, string>
{
    { "ProjectNumber", project.ProjectNumber },
    { "Customer", customer.CompanyName }
};
var metrics = new Dictionary<string, double>
{
    { "ContractAmount", project.ContractAmount.GetValueOrDefault() }
};
_telemetryClient.TrackEvent("ProjectCreated", properties, metrics);

// ❌ DON'T: String concatenation
_logger.LogInformation($"Project {projectId} created");

// ❌ DON'T: Silent failures
try
{
    // code
}
catch (Exception)
{
    // Don't swallow!
}
```

### Log Levels

```csharp
_logger.LogTrace("Detailed diagnostic info");           // Dev only
_logger.LogDebug("Debug-level information");             // Dev/QA
_logger.LogInformation("General informational message"); // Normal operation
_logger.LogWarning("Warning about potential issue");     // Attention needed
_logger.LogError(ex, "Error occurred");                 // Errors
_logger.LogCritical(ex, "Critical system failure");      // System down
```

---

## 7. Testing Strategy

### Unit Tests (xUnit + Moq)

```csharp
[TestFixture]
public class ProjectServiceTests
{
    private Mock<IRepository<Project>> _mockRepository;
    private Mock<ILogger<ProjectService>> _mockLogger;
    private ProjectService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IRepository<Project>>();
        _mockLogger = new Mock<ILogger<ProjectService>>();
        _service = new ProjectService(_mockLogger.Object, _mockRepository.Object);
    }

    [Test]
    public async Task GetProjectAsync_WithValidId_ReturnsProject()
    {
        // Arrange
        var projectId = 1;
        var expectedProject = new Project { ProjectId = 1, JobName = "Test" };
        _mockRepository
            .Setup(r => r.FindAsync(projectId, default))
            .ReturnsAsync(expectedProject);

        // Act
        var result = await _service.GetProjectAsync(projectId);

        // Assert
        result.Should().NotBeNull();
        result.ProjectId.Should().Be(1);
        result.JobName.Should().Be("Test");
    }

    [Test]
    public async Task CreateProjectAsync_WithInvalidData_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateProjectDto { ProjectNumber = "", JobName = "" };

        // Act & Assert
        await _service.Invoking(s => s.CreateProjectAsync(dto))
            .Should()
            .ThrowAsync<ValidationException>();
    }
}
```

### Integration Tests

```csharp
[TestFixture]
public class ProjectApiIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task PostProject_WithValidData_ReturnsCreatedStatus()
    {
        // Arrange
        var request = new CreateProjectDto(
            ProjectNumber: "FW25-TEST",
            JobName: "Test Project",
            ContractAmount: 100000);

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/projects", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}
```

### Component Tests (bUnit)

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
}
```

### Test Coverage Goals

- **Total**: Minimum 80% code coverage
- **Services**: 90%+ (critical business logic)
- **Controllers**: 70%+ (focus on logic, not routing)
- **Components**: 75%+ (focus on user interactions)

---

## 8. Database Migrations

### Creating Migrations

```bash
# Create migration
dotnet ef migrations add AddProjectNotes -p src/Envora.Api -s src/Envora.Api

# View SQL
dotnet ef migrations script 20251218120000 20251218130000 -p src/Envora.Api

# Apply migration
dotnet ef database update -p src/Envora.Api

# Revert migration
dotnet ef database update 20251218110000 -p src/Envora.Api
```

### Migration Best Practices

```csharp
public partial class AddProjectNotes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Notes",
            columns: table => new
            {
                NoteId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectId = table.Column<int>(type: "int", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notes", x => x.NoteId);
                table.ForeignKey(
                    name: "FK_Notes_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Notes_ProjectId",
            table: "Notes",
            column: "ProjectId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Notes");
    }
}
```

---

## 9. API Controller Pattern

```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Authorize] // Require auth on all endpoints unless marked otherwise
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(
        IProjectService projectService,
        ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Get a paginated list of projects
    /// </summary>
    /// <param name="skip">Number of items to skip (pagination)</param>
    /// <param name="take">Number of items to take (max 100)</param>
    /// <param name="status">Filter by project status</param>
    /// <returns>Paginated list of projects</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ProjectListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProjects(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] string status = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var projects = await _projectService.GetProjectsAsync(
                skip, take, status, cancellationToken);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching projects");
            return StatusCode(500, new ErrorResponse("INTERNAL_ERROR", "An error occurred"));
        }
    }

    /// <summary>
    /// Get project by ID
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <returns>Full project details</returns>
    [HttpGet("{projectId}")]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProject(int projectId, CancellationToken cancellationToken = default)
    {
        var project = await _projectService.GetProjectAsync(projectId, cancellationToken);
        if (project == null)
            return NotFound(new ErrorResponse("NOT_FOUND", "Project not found"));
        
        return Ok(project);
    }

    /// <summary>
    /// Create new project
    /// </summary>
    /// <param name="dto">Project data</param>
    /// <returns>Created project</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProject(
        [FromBody] CreateProjectDto dto,
        CancellationToken cancellationToken = default)
    {
        var project = await _projectService.CreateProjectAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetProject), new { projectId = project.ProjectId }, project);
    }

    /// <summary>
    /// Update project
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="dto">Updated project data</param>
    /// <returns>Updated project</returns>
    [HttpPatch("{projectId}")]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProject(
        int projectId,
        [FromBody] UpdateProjectDto dto,
        CancellationToken cancellationToken = default)
    {
        var project = await _projectService.UpdateProjectAsync(projectId, dto, cancellationToken);
        if (project == null)
            return NotFound(new ErrorResponse("NOT_FOUND", "Project not found"));
        
        return Ok(project);
    }

    /// <summary>
    /// Delete project
    /// </summary>
    /// <param name="projectId">Project ID</param>
    [HttpDelete("{projectId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProject(int projectId, CancellationToken cancellationToken = default)
    {
        var success = await _projectService.DeleteProjectAsync(projectId, cancellationToken);
        if (!success)
            return NotFound(new ErrorResponse("NOT_FOUND", "Project not found"));
        
        return NoContent();
    }
}
```

---

## 10. Dependency Injection (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://envora.com", "http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EnvoraDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];
    });

builder.Services.AddAuthorization();

// Application services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IPointService, PointService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<ISubmittalService, SubmittalService>();

// Repositories (generic)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Logging & monitoring
builder.Services.AddApplicationInsightsTelemetry(
    builder.Configuration["ApplicationInsights:InstrumentationKey"]);

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddApplicationInsights();
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Envora Platform API",
        Version = "v1.0"
    });
    
    var xmlFilename = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.MapHub<ProjectHub>("/hubs/project");

app.Run();
```

---

## 11. Configuration Management

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Envora;User Id=sa;Password=..."
  },
  "Auth": {
    "Authority": "https://login.microsoftonline.com/{tenantId}/v2.0",
    "Audience": "api://envora-api",
    "ClientId": "...",
    "ClientSecret": "..."
  },
  "AzureStorage": {
    "ConnectionString": "...",
    "BlobContainerName": "envora-documents"
  },
  "ServiceBus": {
    "ConnectionString": "...",
    "QueueName": "envora-jobs"
  },
  "ApplicationInsights": {
    "InstrumentationKey": "..."
  }
}
```

### Secrets (Azure Key Vault)

Never commit secrets! Use Azure Key Vault:

```bash
# Add secret
az keyvault secret set --vault-name envora-kv --name DbPassword --value "..."

# In app, inject from Key Vault
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://envora-kv.vault.azure.net/"),
    new DefaultAzureCredential());
```

---

## 12. Deployment Pipeline (GitHub Actions)

### .github/workflows/build.yml

```yaml
name: Build & Test

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Run tests
      run: dotnet test --configuration Release --no-build --verbosity normal --logger "trx;LogFileName=test-results.trx"
    
    - name: Upload coverage
      uses: codecov/codecov-action@v3
      with:
        files: ./coverage/coverage.cobertura.xml
    
    - name: Publish API
      run: dotnet publish src/Envora.Api/Envora.Api.csproj -c Release -o api-publish
    
    - name: Upload API artifact
      uses: actions/upload-artifact@v3
      with:
        name: api
        path: api-publish/
```

### .github/workflows/deploy.yml

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    environment: production

    steps:
    - uses: actions/checkout@v3
    
    - name: Login to Azure
      uses: azure/login@v1
      with:
        creds: ${{ secrets.AZURE_CREDENTIALS }}
    
    - name: Deploy to App Service
      uses: azure/webapps-deploy@v2
      with:
        app-name: envora-api
        slot-name: production
        package: ${{ github.workspace }}/api-publish
    
    - name: Run smoke tests
      run: |
        curl -X GET https://api.envora.com/api/v1/health
        echo "Smoke tests passed!"
```

---

## 13. Performance Optimization Checklist

- [ ] Database queries use `.AsNoTracking()` for read-only operations
- [ ] Implement pagination (max 100 items per request)
- [ ] Add caching for frequently accessed data (Redis)
- [ ] Use lazy loading / eager loading appropriately in EF
- [ ] Compress responses (gzip)
- [ ] Implement rate limiting
- [ ] Monitor API response times (Application Insights)
- [ ] Use CDN for static assets
- [ ] Bundle & minify frontend assets
- [ ] Implement SignalR connection pooling

---

## 14. Security Checklist

- [ ] All endpoints require authentication (except /auth/login, /health)
- [ ] Use HTTPS in production (TLS 1.2+)
- [ ] Validate all user input (server-side)
- [ ] Use parameterized queries (EF prevents SQL injection)
- [ ] Sanitize outputs (XSS prevention)
- [ ] Implement CORS carefully (allow only needed origins)
- [ ] Use secure headers (CSP, X-Frame-Options, etc.)
- [ ] Rotate secrets regularly (Key Vault)
- [ ] Log security events (authentication, authorization failures)
- [ ] Run security scans (SonarQube, OWASP ZAP)

---

## 15. Document Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | Dec 18, 2025 | Engineering Team | Initial Development Workflow Guide |

