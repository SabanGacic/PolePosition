namespace PolePosition.API.Models;

public enum SessionStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}

public class Session
{
    public int Id { get; set; }

    public int TrackId { get; set; }
    public Track? Track { get; set; }

    public int SessionTypeId { get; set; }
    public SessionType? SessionType { get; set; }

    public int KartId { get; set; }
    public Kart? Kart { get; set; }

    public string? InstructorId { get; set; }
    public ApplicationUser? Instructor { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Price { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Pending;
}