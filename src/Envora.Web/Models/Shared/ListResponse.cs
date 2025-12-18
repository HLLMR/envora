namespace Envora.Web.Models.Shared;

public sealed class ListResponse<T>
{
    public List<T> Data { get; set; } = [];
}


