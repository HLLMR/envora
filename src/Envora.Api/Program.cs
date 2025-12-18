using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers (spec expects controller pattern + versioned routes)
builder.Services.AddControllers();

// EF Core
builder.Services.AddDbContext<Envora.Api.Data.EnvoraDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// App services
builder.Services.AddScoped<Envora.Api.Services.Interfaces.IProjectService, Envora.Api.Services.Implementations.ProjectService>();
builder.Services.AddScoped<Envora.Api.Services.Interfaces.IEquipmentService, Envora.Api.Services.Implementations.EquipmentService>();
builder.Services.AddScoped<Envora.Api.Services.Interfaces.IPointService, Envora.Api.Services.Implementations.PointService>();
builder.Services.AddScoped<Envora.Api.Services.Interfaces.IDeviceService, Envora.Api.Services.Implementations.DeviceService>();
builder.Services.AddScoped<Envora.Api.Services.Interfaces.IControllerService, Envora.Api.Services.Implementations.ControllerService>();
builder.Services.AddScoped<Envora.Api.Services.Interfaces.INodeService, Envora.Api.Services.Implementations.NodeService>();
builder.Services.AddScoped<Envora.Api.Services.Interfaces.IControllerIoSlotService, Envora.Api.Services.Implementations.ControllerIoSlotService>();

// Swagger/OpenAPI (local dev convenience)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Simple health endpoint for container/orchestrator checks
app.MapGet("/health", () => Results.Ok(new { status = "ok", utc = DateTimeOffset.UtcNow }));

app.Run();
