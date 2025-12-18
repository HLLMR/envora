using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class UpdatePointRequest
{
    [MaxLength(255)]
    public string? PointDescription { get; set; }

    [MaxLength(50)]
    public string? PointType { get; set; }

    [MaxLength(50)]
    public string? DataType { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }
}


