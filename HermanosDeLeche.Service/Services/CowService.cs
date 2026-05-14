using HermanosDeLeche.Domain.DTOs.Cow;
using HermanosDeLeche.Domain.Entities;
using HermanosDeLeche.Domain.Exceptions;
using HermanosDeLeche.Domain.Interfaces;

namespace HermanosDeLeche.Service.Services;

public sealed class CowService : ICowService
{
    private readonly ICowRepository _cows;

    public CowService(ICowRepository cows)
    {
        _cows = cows;
    }

    public async Task<CowResponse> CreateAsync(Guid milkmanId, CreateCowRequest request, CancellationToken ct = default)
    {
        var cow = new Cow
        {
            MilkmanId = milkmanId,
            Nombre = request.Nombre.Trim(),
            FotoUrl = request.FotoUrl?.Trim(),
            Raza = request.Raza?.Trim(),
            Edad = request.Edad,
            Ciudad = request.Ciudad?.Trim(),
            Descripcion = request.Descripcion?.Trim()
        };

        await _cows.CreateAsync(cow, ct);
        return Map(cow);
    }

    public async Task<IReadOnlyList<CowResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await _cows.ListAsync(ct);
        return list.Select(Map).ToList();
    }

    public async Task<CowResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var cow = await _cows.GetByIdAsync(id, ct) ?? throw new AppException("Vaca no encontrada.", 404);
        return Map(cow);
    }

    public async Task<CowResponse> UpdateAsync(Guid milkmanId, Guid cowId, UpdateCowRequest request, CancellationToken ct = default)
    {
        var cow = await _cows.GetByIdAsync(cowId, ct) ?? throw new AppException("Vaca no encontrada.", 404);
        if (cow.MilkmanId != milkmanId)
            throw new AppException("No puedes editar vacas de otro lechero.", 403);

        cow.Nombre = request.Nombre.Trim();
        cow.FotoUrl = request.FotoUrl?.Trim();
        cow.Raza = request.Raza?.Trim();
        cow.Edad = request.Edad;
        cow.Ciudad = request.Ciudad?.Trim();
        cow.Descripcion = request.Descripcion?.Trim();

        var ok = await _cows.UpdateAsync(cow, ct);
        if (!ok) throw new AppException("No se pudo actualizar la vaca.", 409);
        return Map(cow);
    }

    public async Task DeleteAsync(Guid milkmanId, Guid cowId, CancellationToken ct = default)
    {
        var cow = await _cows.GetByIdAsync(cowId, ct) ?? throw new AppException("Vaca no encontrada.", 404);
        if (cow.MilkmanId != milkmanId)
            throw new AppException("No puedes eliminar vacas de otro lechero.", 403);

        var ok = await _cows.DeleteAsync(cowId, ct);
        if (!ok) throw new AppException("No se pudo eliminar la vaca.", 409);
    }

    private static CowResponse Map(Cow cow) => new()
    {
        Id = cow.Id,
        MilkmanId = cow.MilkmanId,
        Nombre = cow.Nombre,
        FotoUrl = cow.FotoUrl,
        Raza = cow.Raza,
        Edad = cow.Edad,
        Ciudad = cow.Ciudad,
        Descripcion = cow.Descripcion,
        FechaRegistro = cow.FechaRegistro
    };
}
