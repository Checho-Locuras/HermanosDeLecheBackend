using HermanosDeLeche.Domain.DTOs.Stats;
using HermanosDeLeche.Domain.Interfaces;

namespace HermanosDeLeche.Service.Services;

public sealed class StatsService : IStatsService
{
    private readonly IStatsRepository _stats;

    public StatsService(IStatsRepository stats)
    {
        _stats = stats;
    }

    public Task<DashboardStatsResponse> GetDashboardAsync(CancellationToken ct = default) =>
        _stats.GetDashboardAsync(ct);

    public Task<IReadOnlyList<RankedCowCountResponse>> GetTopThirstyCowsAsync(CancellationToken ct = default) =>
        _stats.GetTopThirstyCowsAsync(3, ct);

    public Task<IReadOnlyList<RankedCowLitersResponse>> GetTopMilkConsumptionAsync(CancellationToken ct = default) =>
        _stats.GetTopMilkConsumptionAsync(3, ct);

    public Task<IReadOnlyList<RankedMilkmanLitersResponse>> GetTopGenerousMilkmensAsync(CancellationToken ct = default) =>
        _stats.GetTopGenerousMilkmensAsync(3, ct);
}
