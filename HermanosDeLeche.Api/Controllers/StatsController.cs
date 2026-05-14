using HermanosDeLeche.Domain.DTOs.Stats;
using HermanosDeLeche.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HermanosDeLeche.Api.Controllers;

[ApiController]
[Route("api/stats")]
[AllowAnonymous]
public sealed class StatsController : ControllerBase
{
    private readonly IStatsService _stats;

    public StatsController(IStatsService stats)
    {
        _stats = stats;
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardStatsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Dashboard(CancellationToken ct)
    {
        var data = await _stats.GetDashboardAsync(ct);
        return Ok(data);
    }

    [HttpGet("top-thirsty-cows")]
    [ProducesResponseType(typeof(List<RankedCowCountResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TopThirsty(CancellationToken ct)
    {
        var list = await _stats.GetTopThirstyCowsAsync(ct);
        return Ok(list);
    }

    [HttpGet("top-milk-consumption")]
    [ProducesResponseType(typeof(List<RankedCowLitersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TopConsumption(CancellationToken ct)
    {
        var list = await _stats.GetTopMilkConsumptionAsync(ct);
        return Ok(list);
    }

    [HttpGet("top-generous-milkmen")]
    [ProducesResponseType(typeof(List<RankedMilkmanLitersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TopGenerous(CancellationToken ct)
    {
        var list = await _stats.GetTopGenerousMilkmensAsync(ct);
        return Ok(list);
    }
}
