namespace HermanosDeLeche.Domain.Entities;

public sealed class CowMilkIntake
{
    public Guid Id { get; set; }
    public Guid CowId { get; set; }
    public Guid MilkmanId { get; set; }
    public decimal CantidadLitros { get; set; }
    public DateTimeOffset Fecha { get; set; }
    public string? Observaciones { get; set; }
}
