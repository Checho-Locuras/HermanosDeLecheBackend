using HermanosDeLeche.Domain.DTOs.Milkman;
using HermanosDeLeche.Domain.DTOs.Stats;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IMilkmanService
{
    Task<IReadOnlyList<MilkmanResponse>> ListAsync(CancellationToken ct = default);
    Task<MilkmanResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
}
