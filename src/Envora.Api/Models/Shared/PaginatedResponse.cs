namespace Envora.Api.Models.Shared;

public sealed record PaginatedResponse<T>(
    IReadOnlyList<T> Data,
    int Skip,
    int Take,
    int Total
);


