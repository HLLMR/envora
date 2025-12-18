namespace Envora.Api.Data.Entities;

public sealed class PointDistribution
{
    public Guid DistributionId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid SoftPointId { get; set; }
    public Guid ConsumingControllerId { get; set; }
    public Guid? NodeId { get; set; }

    public string? LocalPointName { get; set; }
    public DateTime CreatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public Point SoftPoint { get; set; } = null!;
    public Controller ConsumingController { get; set; } = null!;
    public Node? Node { get; set; }
}


