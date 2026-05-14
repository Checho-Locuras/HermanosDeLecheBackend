namespace HermanosDeLeche.Domain.Entities;

public sealed class Cow
{
    public Guid Id { get; set; }
    public Guid MilkmanId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? FotoUrl { get; set; }
    public string? Raza { get; set; }
    public int? Edad { get; set; }
    public string? Ciudad { get; set; }
    public string? Descripcion { get; set; }
    public DateTimeOffset FechaRegistro { get; set; }
}
