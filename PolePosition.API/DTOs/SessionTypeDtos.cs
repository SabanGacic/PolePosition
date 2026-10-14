using System.ComponentModel.DataAnnotations;
using PolePosition.API.Common;

namespace PolePosition.API.DTOs;

public class SessionTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class SessionTypeUpsertDto
{
    [Required(ErrorMessage = "Unesite naziv tipa sesije (najviše 100 znakova).")]
    [StringLength(100, ErrorMessage = "Naziv tipa sesije može imati najviše 100 znakova.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Opis može imati najviše 500 znakova.")]
    public string? Description { get; set; }
}

public class SessionTypeSearchQuery : PagedQuery
{
    public string? Name { get; set; }
}