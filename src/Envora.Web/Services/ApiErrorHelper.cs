using System.Net;
using Envora.Web.Models.Shared;

namespace Envora.Web.Services;

public static class ApiErrorHelper
{
    public static async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken ct)
    {
        ApiErrorResponse? errorResponse = null;
        
        // Only try to read JSON if there's content
        if (response.Content.Headers.ContentLength > 0)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync(ct);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(cancellationToken: ct);
                }
            }
            catch (System.Text.Json.JsonException)
            {
                // If JSON parsing fails, continue with default error message
            }
        }
        
        var error = errorResponse?.Error;
        
        if (error != null)
        {
            var validationErrors = error.Details?.Select(d => $"{d.Field}: {d.Message}").ToList();
            throw new ApiException(
                error.Message ?? "An error occurred",
                error.StatusCode,
                error.Code,
                validationErrors
            );
        }

        var message = response.StatusCode switch
        {
            HttpStatusCode.NotFound => "Resource not found",
            HttpStatusCode.BadRequest => "Invalid request",
            HttpStatusCode.Unauthorized => "Unauthorized",
            HttpStatusCode.Forbidden => "Forbidden",
            HttpStatusCode.InternalServerError => "Server error occurred",
            _ => $"Request failed with status {response.StatusCode}"
        };

        throw new ApiException(message, (int)response.StatusCode);
    }
}

