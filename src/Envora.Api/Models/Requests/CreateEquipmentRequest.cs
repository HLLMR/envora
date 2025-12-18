using System.ComponentModel.DataAnnotations;

namespace Envora.Api.Models.Requests;

public sealed class CreateEquipmentRequest
{
    [Required]
    [MaxLength(50)]
    public string EquipmentTag { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string EquipmentType { get; set; } = null!;

    [MaxLength(100)]
    public string? Manufacturer { get; set; }

    [MaxLength(100)]
    public string? Model { get; set; }

    [MaxLength(255)]
    public string? Location { get; set; }

    public string? Description { get; set; }
}


