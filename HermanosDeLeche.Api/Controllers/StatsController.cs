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
    public async Task<IActionResult> Dashboard(CancellationToken ct)
    {
        var data = await _stats.GetDashboardAsync(ct);
        return Ok(data);
    }

    [HttpGet("top-thirsty-cows")]
    public async Task<IActionResult> TopThirsty(CancellationToken ct)
    {
        var list = await _stats.GetTopThirstyCowsAsync(ct);
        return Ok(list);
    }

    [HttpGet("top-milk-consumption")]
    public async Task<IActionResult> TopConsumption(CancellationToken ct)
    {
        var list = await _stats.GetTopMilkConsumptionAsync(ct);
        return Ok(list);
    }

    [HttpGet("top-generous-milkmen")]
    public async Task<IActionResult> TopGenerous(CancellationToken ct)
    {
        var list = await _stats.GetTopGenerousMilkmensAsync(ct);
        return Ok(list);
    }
}
