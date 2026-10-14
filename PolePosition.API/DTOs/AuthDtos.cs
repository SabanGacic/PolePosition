using System.ComponentModel.DataAnnotations;

namespace PolePosition.API.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Unesite ime (najviše 50 znakova).")]
    [StringLength(50, ErrorMessage = "Ime može imati najviše 50 znakova.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unesite prezime (najviše 50 znakova).")]
    [StringLength(50, ErrorMessage = "Prezime može imati najviše 50 znakova.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unesite e-mail adresu.")]
    [EmailAddress(ErrorMessage = "Unesite validnu e-mail adresu u formatu: korisnik@domena.com.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unesite lozinku.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Lozinka mora imati najmanje 8 znakova, uključujući veliko slovo, malo slovo i broj.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required(ErrorMessage = "Unesite e-mail adresu.")]
    [EmailAddress(ErrorMessage = "Unesite validnu e-mail adresu u formatu: korisnik@domena.com.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unesite lozinku.")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}