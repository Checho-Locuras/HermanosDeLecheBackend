using HermanosDeLeche.Domain.Entities;

namespace HermanosDeLeche.Domain.Interfaces;

public interface ICowRepository
{
    Task<Cow?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Cow>> ListAsync(CancellationToken ct = default);
    Task<Cow> CreateAsync(Cow cow, CancellationToken ct = default);
    Task<bool> UpdateAsync(Cow cow, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Cow>> ListFedByMilkmanAsync(Guid milkmanId, CancellationToken ct = default);
}
