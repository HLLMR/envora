namespace Envora.Web.Models.Shared;

public sealed class ApiErrorResponse
{
    public ApiError? Error { get; set; }
}

public sealed class ApiError
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    public string? Timestamp { get; set; }
    public List<ApiErrorDetail>? Details { get; set; }
}

public sealed class ApiErrorDetail
{
    public string? Field { get; set; }
    public string? Message { get; set; }
}

