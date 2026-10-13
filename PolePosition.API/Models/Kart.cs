namespace PolePosition.API.Models;

public enum KartStatus
{
    Available,
    InMaintenance,
    OutOfService
}

public class Kart
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int TrackId { get; set; }
    public Track? Track { get; set; }
    public KartStatus Status { get; set; } = KartStatus.Available;
}