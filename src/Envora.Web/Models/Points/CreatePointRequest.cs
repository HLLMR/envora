namespace Envora.Web.Models.Points;

public sealed class CreatePointRequest
{
    public string PointTag { get; set; } = "";
    public string? PointDescription { get; set; }
    public string SourceType { get; set; } = "SoftPoint";
    public string PointType { get; set; } = "Input";
    public string DataType { get; set; } = "";
    public string? Unit { get; set; }
}


