namespace Envora.Api.Data.Entities;

public sealed class ProjectDocument
{
    public Guid DocumentId { get; set; }
    public Guid ProjectId { get; set; }

    public string? DocumentType { get; set; }
    public string DocumentName { get; set; } = null!;
    public string? Description { get; set; }
    public string BlobStorageUrl { get; set; } = null!;
    public long? FileSize { get; set; }
    public string? FileType { get; set; }
    public Guid UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public int? Version { get; set; }
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public User UploadedByUser { get; set; } = null!;
}


