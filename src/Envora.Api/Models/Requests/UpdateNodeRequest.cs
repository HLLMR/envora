using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class UpdateNodeRequest
{
    [MaxLength(100)]
    public string? NodeName { get; set; }

    [MaxLength(100)]
    public string? NodeType { get; set; }

    [MaxLength(50)]
    public string? Protocol { get; set; }

    [MaxLength(50)]
    public string? NetworkAddress { get; set; }

    public bool? IsActive { get; set; }
}


