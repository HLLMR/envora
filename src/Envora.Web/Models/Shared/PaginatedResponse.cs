namespace Envora.Web.Models.Shared;

public sealed class PaginatedResponse<T>
{
    public List<T> Data { get; set; } = [];
    public int Skip { get; set; }
    public int Take { get; set; }
    public int Total { get; set; }
}


