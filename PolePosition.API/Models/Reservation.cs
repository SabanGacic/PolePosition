namespace PolePosition.API.Models;

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}

public class Reservation
{
    public int Id { get; set; }

    public string DriverId { get; set; } = string.Empty;
    public ApplicationUser? Driver { get; set; }

    public int SessionId { get; set; }
    public Session? Session { get; set; }

    public decimal TotalPrice { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    // Audit fields: who approved/rejected and why
    public string? ReviewedByUserId { get; set; }
    public ApplicationUser? ReviewedByUser { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ReservationEquipment> ReservationEquipments { get; set; } = new List<ReservationEquipment>();
}