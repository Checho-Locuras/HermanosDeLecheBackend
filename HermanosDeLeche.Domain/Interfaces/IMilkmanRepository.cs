using HermanosDeLeche.Domain.Entities;

namespace HermanosDeLeche.Domain.Interfaces;

public interface IMilkmanRepository
{
    Task<Milkman?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Milkman?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<Milkman?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<Milkman>> ListAsync(CancellationToken ct = default);
    Task<Milkman> CreateAsync(Milkman milkman, CancellationToken ct = default);
}
