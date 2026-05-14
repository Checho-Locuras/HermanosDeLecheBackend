using System.ComponentModel.DataAnnotations;

namespace HermanosDeLeche.Domain.DTOs.Cow;

public sealed class UpdateCowRequest
{
    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    public string? FotoUrl { get; set; }

    [MaxLength(100)]
    public string? Tamano { get; set; }

    [Range(0.01, 1500)]
    public decimal? Peso { get; set; }

    [MaxLength(100)]
    public string? Color { get; set; }

    [Range(0, 50)]
    public int? Edad { get; set; }

    [MaxLength(200)]
    public string? Ciudad { get; set; }

    public string? Descripcion { get; set; }
}
