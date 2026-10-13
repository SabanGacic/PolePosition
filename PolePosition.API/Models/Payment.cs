namespace PolePosition.API.Models;

public enum PaymentStatus
{
    Pending,
    Completed,
    Refunded,
    Failed
}

public class Payment
{
    public int Id { get; set; }

    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public decimal Amount { get; set; }
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public decimal? RefundedAmount { get; set; }
    public DateTime? RefundedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}