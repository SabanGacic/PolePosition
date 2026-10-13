namespace PolePosition.API.Models;

public class IncidentRecord
{
    public int Id { get; set; }

    public int KartId { get; set; }
    public Kart? Kart { get; set; }

    public int? SessionId { get; set; }
    public Session? Session { get; set; }

    public string Description { get; set; } = string.Empty;
    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
}