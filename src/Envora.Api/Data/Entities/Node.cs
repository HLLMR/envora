namespace Envora.Api.Data.Entities;

public sealed class Node
{
    public Guid NodeId { get; set; }
    public Guid ControllerId { get; set; }
    public Guid ProjectId { get; set; }

    public string NodeName { get; set; } = null!;
    public string NodeType { get; set; } = null!;

    public string? Protocol { get; set; }
    public string? NetworkAddress { get; set; }
    public int? MaxDevices { get; set; }
    public string? BusAssociation { get; set; }

    public bool? IsActive { get; set; }
    public DateOnly? CommissioningDate { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public Controller Controller { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public User? CreatedByUser { get; set; }
}


