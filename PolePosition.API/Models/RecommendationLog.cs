namespace PolePosition.API.Models;

public class RecommendationLog
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public int RecommendedSessionTypeId { get; set; }
    public SessionType? RecommendedSessionType { get; set; }

    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}