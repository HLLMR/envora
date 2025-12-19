namespace Envora.Web.Services;

public sealed class ApiException : Exception
{
    public int StatusCode { get; }
    public string? ErrorCode { get; }
    public List<string>? ValidationErrors { get; }

    public ApiException(string message, int statusCode, string? errorCode = null, List<string>? validationErrors = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors;
    }
}

