using HermanosDeLeche.Domain.Entities;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IIntakeRepository
{
    Task<CowMilkIntake> CreateAsync(CowMilkIntake intake, CancellationToken ct = default);
    Task<IReadOnlyList<CowMilkIntake>> ListAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CowMilkIntake>> ListByCowAsync(Guid cowId, CancellationToken ct = default);
    Task<IReadOnlyList<CowMilkIntake>> ListByMilkmanAsync(Guid milkmanId, CancellationToken ct = default);
}
