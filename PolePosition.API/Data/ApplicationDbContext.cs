using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PolePosition.API.Models;
using Microsoft.AspNetCore.Identity;

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
    public DbSet<RevokedToken> RevokedTokens => Set<RevokedToken>();

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

        builder.Entity<RevokedToken>().HasIndex(t => t.Jti).IsUnique();
        builder.Entity<RevokedToken>().Property(t => t.Jti).HasMaxLength(64);

        // Seed roles
        var adminRoleId = "1";
        var driverRoleId = "2";
        var instructorRoleId = "3";

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = driverRoleId, Name = "Driver", NormalizedName = "DRIVER" },
            new IdentityRole { Id = instructorRoleId, Name = "Instructor", NormalizedName = "INSTRUCTOR" }
        );

        // Seed reference data
        builder.Entity<Track>().HasData(
            new Track { Id = 1, Name = "Kart Arena Bugojno", Location = "Bugojno", LengthMeters = 850 }
        );

        builder.Entity<SessionType>().HasData(
            new SessionType { Id = 1, Name = "Slobodna voznja", Description = "Opusten krug bez pritiska vremena" },
            new SessionType { Id = 2, Name = "Trka", Description = "Takmicarska sesija sa rang listom" },
            new SessionType { Id = 3, Name = "Obuka", Description = "Sesija sa instruktorom za pocetnike" }
        );

        builder.Entity<EquipmentType>().HasData(
            new EquipmentType { Id = 1, Name = "Kaciga", AdditionalPrice = 0 },
            new EquipmentType { Id = 2, Name = "Kombinezon", AdditionalPrice = 5 }
        );

        builder.Entity<Kart>().HasData(
            new Kart { Id = 1, Code = "K-01", Model = "Sodikart RT8", TrackId = 1, Status = KartStatus.Available },
            new Kart { Id = 2, Code = "K-02", Model = "Sodikart RT8", TrackId = 1, Status = KartStatus.Available }
        );
    }
}