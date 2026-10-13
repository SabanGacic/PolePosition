using Microsoft.AspNetCore.Identity;

namespace PolePosition.API.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  
    public double AverageLapTime { get; set; }
    public int TotalSessionsCompleted { get; set; }
}