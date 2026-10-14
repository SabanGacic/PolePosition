using System.ComponentModel.DataAnnotations;
using PolePosition.API.Common;

namespace PolePosition.API.DTOs;

public class EquipmentTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
}

public class EquipmentTypeUpsertDto
{
    [Required(ErrorMessage = "Unesite naziv opreme (najviše 100 znakova).")]
    [StringLength(100, ErrorMessage = "Naziv opreme može imati najviše 100 znakova.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 1000, ErrorMessage = "Dodatna cijena mora biti između 0 i 1000 KM.")]
    public decimal AdditionalPrice { get; set; }
}

public class EquipmentTypeSearchQuery : PagedQuery
{
    public string? Name { get; set; }
}