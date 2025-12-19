namespace Envora.Web.Models.Points;

public sealed class UpdatePointRequest
{
    public string? PointDescription { get; set; }
    public string? PointType { get; set; }
    public string? DataType { get; set; }
    public string? Unit { get; set; }
}

