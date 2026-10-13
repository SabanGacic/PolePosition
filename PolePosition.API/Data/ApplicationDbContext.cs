using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PolePosition.API.Models;

namespace PolePosition.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Kart> Karts => Set<Kart>();
    public DbSet<SessionType> SessionTypes => Set<SessionType>();
    public DbSet<EquipmentType> EquipmentTypes => Set<EquipmentType>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationEquipment> ReservationEquipments => Set<ReservationEquipment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<LapResult> LapResults => Set<LapResult>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<IncidentRecord> IncidentRecords => Set<IncidentRecord>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<RecommendationLog> RecommendationLogs => Set<RecommendationLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Composite key for the many-to-many join table
        builder.Entity<ReservationEquipment>()
            .HasKey(re => new { re.ReservationId, re.EquipmentTypeId });

        builder.Entity<ReservationEquipment>()
            .HasOne(re => re.Reservation)
            .WithMany(r => r.ReservationEquipments)
            .HasForeignKey(re => re.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ReservationEquipment>()
            .HasOne(re => re.EquipmentType)
            .WithMany()
            .HasForeignKey(re => re.EquipmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent cascade cycles on Session relations (Instructor, Kart)
        builder.Entity<Session>()
            .HasOne(s => s.Instructor)
            .WithMany()
            .HasForeignKey(s => s.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Session>()
            .HasOne(s => s.Kart)
            .WithMany()
            .HasForeignKey(s => s.KartId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reservation>()
            .HasOne(r => r.ReviewedByUser)
            .WithMany()
            .HasForeignKey(r => r.ReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reservation>()
            .HasOne(r => r.Driver)
            .WithMany()
            .HasForeignKey(r => r.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Decimal precision for money fields
        builder.Entity<Reservation>().Property(r => r.TotalPrice).HasPrecision(10, 2);
        builder.Entity<Session>().Property(s => s.Price).HasPrecision(10, 2);
        builder.Entity<Payment>().Property(p => p.Amount).HasPrecision(10, 2);
        builder.Entity<Payment>().Property(p => p.RefundedAmount).HasPrecision(10, 2);
        builder.Entity<EquipmentType>().Property(e => e.AdditionalPrice).HasPrecision(10, 2);
    }
}