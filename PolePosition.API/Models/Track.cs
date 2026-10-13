namespace PolePosition.API.Models;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double LengthMeters { get; set; }
}