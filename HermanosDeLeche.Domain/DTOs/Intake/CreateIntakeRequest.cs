using System.ComponentModel.DataAnnotations;

namespace HermanosDeLeche.Domain.DTOs.Intake;

public sealed class CreateIntakeRequest
{
    [Required]
    public Guid CowId { get; set; }

    [Range(0.01, 10000)]
    public decimal CantidadLitros { get; set; }

    public DateTimeOffset? Fecha { get; set; }

    public string? Observaciones { get; set; }
}
