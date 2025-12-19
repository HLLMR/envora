namespace Envora.Web.Models.Equipment;

public sealed class UpdateEquipmentRequest
{
    public string? EquipmentTag { get; set; }
    public string? EquipmentType { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
}

