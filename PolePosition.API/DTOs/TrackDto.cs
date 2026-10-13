using System.ComponentModel.DataAnnotations;
using PolePosition.API.Common;

namespace PolePosition.API.DTOs;

public class TrackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double LengthMeters { get; set; }
}

public class TrackUpsertDto
{
    [Required(ErrorMessage = "Unesite naziv staze (najviše 100 znakova).")]
    [StringLength(100, ErrorMessage = "Naziv staze može imati najviše 100 znakova.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unesite lokaciju staze (najviše 150 znakova).")]
    [StringLength(150, ErrorMessage = "Lokacija može imati najviše 150 znakova.")]
    public string Location { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Opis može imati najviše 500 znakova.")]
    public string? Description { get; set; }

    [Range(50, 5000, ErrorMessage = "Dužina staze mora biti između 50 i 5000 metara.")]
    public double LengthMeters { get; set; }
}

public class TrackSearchQuery : PagedQuery
{
    public string? Name { get; set; }
}