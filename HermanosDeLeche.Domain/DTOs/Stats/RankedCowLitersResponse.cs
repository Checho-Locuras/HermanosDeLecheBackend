namespace HermanosDeLeche.Domain.DTOs.Stats;

public sealed class RankedCowLitersResponse
{
    public Guid CowId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal TotalLitros { get; set; }
}
