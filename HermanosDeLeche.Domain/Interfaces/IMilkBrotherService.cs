using HermanosDeLeche.Domain.DTOs.Milkman;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IMilkBrotherService
{
    Task<IReadOnlyList<MilkBrotherResponse>> GetMyMilkBrothersAsync(Guid milkmanId, CancellationToken ct = default);
}
