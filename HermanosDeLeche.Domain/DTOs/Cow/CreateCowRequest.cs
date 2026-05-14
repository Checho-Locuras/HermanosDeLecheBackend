using System.ComponentModel.DataAnnotations;

namespace HermanosDeLeche.Domain.DTOs.Cow;

public sealed class CreateCowRequest
{
    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    public string? FotoUrl { get; set; }

    [MaxLength(100)]
    public string? Raza { get; set; }

    [Range(0, 50)]
    public int? Edad { get; set; }

    [MaxLength(200)]
    public string? Ciudad { get; set; }

    public string? Descripcion { get; set; }
}
