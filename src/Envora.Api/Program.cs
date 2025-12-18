var builder = WebApplication.CreateBuilder(args);

// Controllers (spec expects controller pattern + versioned routes)
builder.Services.AddControllers();

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
