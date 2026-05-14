using HermanosDeLeche.Domain.DTOs.Intake;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IIntakeService
{
    Task<IntakeResponse> CreateAsync(Guid milkmanId, CreateIntakeRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<IntakeResponse>> ListAsync(CancellationToken ct = default);
    Task<IReadOnlyList<IntakeResponse>> ListByCowAsync(Guid cowId, CancellationToken ct = default);
    Task<IReadOnlyList<IntakeResponse>> ListByMilkmanAsync(Guid milkmanId, CancellationToken ct = default);
}
