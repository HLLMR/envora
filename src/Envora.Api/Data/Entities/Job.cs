namespace Envora.Api.Data.Entities;

public sealed class Job
{
    public Guid JobId { get; set; }
    public Guid ProjectId { get; set; }

    public string JobType { get; set; } = null!;
    public string Status { get; set; } = null!;

    public string? Parameters { get; set; }
    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }

    public int? RetryCount { get; set; }
    public int? MaxRetries { get; set; }

    public Guid RequestedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public User RequestedByUser { get; set; } = null!;
}


