using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class CreateControllerRequest
{
    [Required]
    [MaxLength(100)]
    public string ControllerName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string ControllerType { get; set; } = null!;

    [MaxLength(100)]
    public string? Manufacturer { get; set; }

    [MaxLength(100)]
    public string? Model { get; set; }
}


