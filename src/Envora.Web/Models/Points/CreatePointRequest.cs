using System.ComponentModel.DataAnnotations;

namespace Envora.Web.Models.Points;

public sealed class CreatePointRequest
{
    [Required]
    [MaxLength(100)]
    public string PointTag { get; set; } = "";

    [MaxLength(255)]
    public string? PointDescription { get; set; }

    [Required]
    [MaxLength(50)]
    public string SourceType { get; set; } = "SoftPoint";

    [Required]
    [MaxLength(50)]
    public string PointType { get; set; } = "Input";

    [Required]
    [MaxLength(50)]
    public string DataType { get; set; } = "";

    [MaxLength(50)]
    public string? Unit { get; set; }
}


