using HermanosDeLeche.Domain.DTOs.Milkman;
using HermanosDeLeche.Domain.Exceptions;
using HermanosDeLeche.Domain.Interfaces;

namespace HermanosDeLeche.Service.Services;

public sealed class MilkmanService : IMilkmanService
{
    private readonly IMilkmanRepository _milkmen;

    public MilkmanService(IMilkmanRepository milkmen)
    {
        _milkmen = milkmen;
    }

    public async Task<IReadOnlyList<MilkmanResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await _milkmen.ListAsync(ct);
        return list.Select(m => new MilkmanResponse
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Username = m.Username,
            Email = m.Email,
            Ciudad = m.Ciudad,
            FotoUrl = m.FotoUrl,
            FechaRegistro = m.FechaRegistro
        }).ToList();
    }

    public async Task<MilkmanResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var m = await _milkmen.GetByIdAsync(id, ct) ?? throw new AppException("Lechero no encontrado.", 404);
        return new MilkmanResponse
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Username = m.Username,
            Email = m.Email,
            Ciudad = m.Ciudad,
            FotoUrl = m.FotoUrl,
            FechaRegistro = m.FechaRegistro
        };
    }
}
