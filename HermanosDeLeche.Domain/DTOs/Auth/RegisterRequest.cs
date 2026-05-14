using System.ComponentModel.DataAnnotations;

namespace HermanosDeLeche.Domain.DTOs.Auth;

public sealed class RegisterRequest
{
    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Ciudad { get; set; }

    public string? FotoUrl { get; set; }
}
