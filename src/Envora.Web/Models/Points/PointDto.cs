namespace Envora.Web.Models.Points;

public sealed class PointDto
{
    public Guid PointId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid EquipmentId { get; set; }
    public string PointTag { get; set; } = "";
    public string SourceType { get; set; } = "";
    public string PointType { get; set; } = "";
    public string DataType { get; set; } = "";
    public string? Unit { get; set; }
    public string? PointDescription { get; set; }
}


