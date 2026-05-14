using HermanosDeLeche.Domain.DTOs.Intake;
using HermanosDeLeche.Domain.Entities;
using HermanosDeLeche.Domain.Exceptions;
using HermanosDeLeche.Domain.Interfaces;

namespace HermanosDeLeche.Service.Services;

public sealed class IntakeService : IIntakeService
{
    private readonly IIntakeRepository _intakes;
    private readonly ICowRepository _cows;

    public IntakeService(IIntakeRepository intakes, ICowRepository cows)
    {
        _intakes = intakes;
        _cows = cows;
    }

    public async Task<IntakeResponse> CreateAsync(Guid milkmanId, CreateIntakeRequest request, CancellationToken ct = default)
    {
        _ = await _cows.GetByIdAsync(request.CowId, ct) ?? throw new AppException("Vaca no encontrada.", 404);

        var intake = new CowMilkIntake
        {
            CowId = request.CowId,
            MilkmanId = milkmanId,
            CantidadLitros = request.CantidadLitros,
            Fecha = request.Fecha ?? DateTimeOffset.UtcNow,
            Observaciones = request.Observaciones?.Trim()
        };

        await _intakes.CreateAsync(intake, ct);
        return Map(intake);
    }

    public async Task<IReadOnlyList<IntakeResponse>> ListAsync(CancellationToken ct = default)
    {
        var list = await _intakes.ListAsync(ct);
        return list.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<IntakeResponse>> ListByCowAsync(Guid cowId, CancellationToken ct = default)
    {
        var list = await _intakes.ListByCowAsync(cowId, ct);
        return list.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<IntakeResponse>> ListByMilkmanAsync(Guid milkmanId, CancellationToken ct = default)
    {
        var list = await _intakes.ListByMilkmanAsync(milkmanId, ct);
        return list.Select(Map).ToList();
    }

    private static IntakeResponse Map(CowMilkIntake i) => new()
    {
        Id = i.Id,
        CowId = i.CowId,
        MilkmanId = i.MilkmanId,
        CantidadLitros = i.CantidadLitros,
        Fecha = i.Fecha,
        Observaciones = i.Observaciones
    };
}
