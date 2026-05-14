using HermanosDeLeche.Domain.DTOs.Stats;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IStatsService
{
    Task<DashboardStatsResponse> GetDashboardAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RankedCowCountResponse>> GetTopThirstyCowsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RankedCowLitersResponse>> GetTopMilkConsumptionAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RankedMilkmanLitersResponse>> GetTopGenerousMilkmensAsync(CancellationToken ct = default);
}
