using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class CreateNodeRequest
{
    [Required]
    [MaxLength(100)]
    public string NodeName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string NodeType { get; set; } = null!;

    [MaxLength(50)]
    public string? Protocol { get; set; }

    [MaxLength(50)]
    public string? NetworkAddress { get; set; }
}


