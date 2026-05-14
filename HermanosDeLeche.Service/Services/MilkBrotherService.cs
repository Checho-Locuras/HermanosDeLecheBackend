using HermanosDeLeche.Domain.DTOs.Milkman;
using HermanosDeLeche.Domain.Interfaces;

namespace HermanosDeLeche.Service.Services;

public sealed class MilkBrotherService : IMilkBrotherService
{
    private readonly IMilkBrotherRepository _repo;

    public MilkBrotherService(IMilkBrotherRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<MilkBrotherResponse>> GetMyMilkBrothersAsync(Guid milkmanId, CancellationToken ct = default) =>
        _repo.ListForMilkmanAsync(milkmanId, ct);
}
