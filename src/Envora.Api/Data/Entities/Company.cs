namespace Envora.Api.Data.Entities;

public sealed class Company
{
    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


