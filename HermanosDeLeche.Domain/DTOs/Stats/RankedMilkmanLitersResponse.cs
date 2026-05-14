namespace HermanosDeLeche.Domain.DTOs.Stats;

public sealed class RankedMilkmanLitersResponse
{
    public Guid MilkmanId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public decimal TotalLitros { get; set; }
}
