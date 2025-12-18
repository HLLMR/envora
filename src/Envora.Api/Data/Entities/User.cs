namespace Envora.Api.Data.Entities;

public sealed class User
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = null!;
    public string? Company { get; set; }
    public bool IsActive { get; set; }
    public string? AzureAdId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}


