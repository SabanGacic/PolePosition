namespace PolePosition.API.Models;

public class ReservationEquipment
{
    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int EquipmentTypeId { get; set; }
    public EquipmentType? EquipmentType { get; set; }
}