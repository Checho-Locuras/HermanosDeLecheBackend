namespace HermanosDeLeche.Domain.DTOs.Milkman;

public sealed class MilkmanResponse
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Ciudad { get; set; }
    public string? FotoUrl { get; set; }
    public DateTimeOffset FechaRegistro { get; set; }
}
