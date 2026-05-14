using HermanosDeLeche.Domain.DTOs.Stats;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IStatsRepository
{
    Task<DashboardStatsResponse> GetDashboardAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RankedCowCountResponse>> GetTopThirstyCowsAsync(int take, CancellationToken ct = default);
    Task<IReadOnlyList<RankedCowLitersResponse>> GetTopMilkConsumptionAsync(int take, CancellationToken ct = default);
    Task<IReadOnlyList<RankedMilkmanLitersResponse>> GetTopGenerousMilkmensAsync(int take, CancellationToken ct = default);
}
