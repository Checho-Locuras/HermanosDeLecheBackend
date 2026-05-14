namespace HermanosDeLeche.Domain.DTOs.Stats;

public sealed class RankedCowCountResponse
{
    public Guid CowId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public long Veces { get; set; }
}
