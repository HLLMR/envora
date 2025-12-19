namespace Envora.Web.Services;

public interface IApiHealthService
{
    Task<bool> CheckHealthAsync(CancellationToken ct = default);
    bool IsHealthy { get; }
    DateTime? LastChecked { get; }
}

public sealed class ApiHealthService(IHttpClientFactory httpClientFactory) : IApiHealthService
{
    public bool IsHealthy { get; private set; }
    public DateTime? LastChecked { get; private set; }

    public async Task<bool> CheckHealthAsync(CancellationToken ct = default)
    {
        try
        {
            var http = httpClientFactory.CreateClient("EnvoraApi");
            var response = await http.GetAsync("health", ct);
            IsHealthy = response.IsSuccessStatusCode;
            LastChecked = DateTime.UtcNow;
            return IsHealthy;
        }
        catch
        {
            IsHealthy = false;
            LastChecked = DateTime.UtcNow;
            return false;
        }
    }
}

