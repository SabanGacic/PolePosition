namespace PolePosition.API.Models;

public class MaintenanceRecord
{
    public int Id { get; set; }

    public int KartId { get; set; }
    public Kart? Kart { get; set; }

    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Description { get; set; } = string.Empty;
}