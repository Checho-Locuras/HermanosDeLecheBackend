using HermanosDeLeche.Domain.DTOs.Milkman;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IMilkBrotherRepository
{
    Task<IReadOnlyList<MilkBrotherResponse>> ListForMilkmanAsync(Guid milkmanId, CancellationToken ct = default);
}
