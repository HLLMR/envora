using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class UpdateControllerRequest
{
    [MaxLength(100)]
    public string? ControllerName { get; set; }

    [MaxLength(100)]
    public string? ControllerType { get; set; }

    [MaxLength(100)]
    public string? Manufacturer { get; set; }

    [MaxLength(100)]
    public string? Model { get; set; }

    public bool? IsActive { get; set; }
}


