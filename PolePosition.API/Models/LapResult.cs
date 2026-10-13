namespace PolePosition.API.Models;

public class LapResult
{
    public int Id { get; set; }

    public int SessionId { get; set; }
    public Session? Session { get; set; }

    public string DriverId { get; set; } = string.Empty;
    public ApplicationUser? Driver { get; set; }

    public TimeSpan LapTime { get; set; }
    public int Position { get; set; }
}