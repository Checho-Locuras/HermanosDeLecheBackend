namespace HermanosDeLeche.Domain.DTOs.Stats;

public sealed class DashboardStatsResponse
{
    public long TotalVacas { get; set; }
    public long TotalLecheros { get; set; }
    public decimal TotalLitrosConsumidos { get; set; }
    public long TotalRegistrosIngestas { get; set; }
}
