using HermanosDeLeche.Domain.DTOs.Cow;

namespace HermanosDeLeche.Domain.Interfaces;

public interface ICowService
{
    Task<CowResponse> CreateAsync(Guid milkmanId, CreateCowRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<CowResponse>> ListAsync(CancellationToken ct = default);
    Task<CowResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CowResponse> UpdateAsync(Guid milkmanId, Guid cowId, UpdateCowRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid milkmanId, Guid cowId, CancellationToken ct = default);
    Task<IReadOnlyList<CowResponse>> ListFedByMilkmanAsync(Guid milkmanId, CancellationToken ct = default);
}
