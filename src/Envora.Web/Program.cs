using Envora.Web.Components;
using Envora.Web.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOptions<EnvoraApiOptions>()
    .Bind(builder.Configuration.GetSection(EnvoraApiOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiBaseUrl), $"{EnvoraApiOptions.SectionName}:ApiBaseUrl is required");

builder.Services.AddHttpClient("EnvoraApi", (sp, http) =>
{
    var options = sp.GetRequiredService<IOptions<EnvoraApiOptions>>().Value;
    http.BaseAddress = new Uri(options.ApiBaseUrl.TrimEnd('/') + "/");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("EnvoraApi"));

builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IPointsService, PointsService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IHubConnectionService, HubConnectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
