using HermanosDeLeche.Api.Security;
using HermanosDeLeche.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HermanosDeLeche.Api.Controllers;

[ApiController]
[Route("api/milkmen")]
public sealed class MilkmenController : ControllerBase
{
    private readonly IMilkmanService _milkmen;
    private readonly IMilkBrotherService _milkBrothers;

    public MilkmenController(IMilkmanService milkmen, IMilkBrotherService milkBrothers)
    {
        _milkmen = milkmen;
        _milkBrothers = milkBrothers;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _milkmen.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("hermanos-de-leche")]
    [Authorize]
    public async Task<IActionResult> MyMilkBrothers(CancellationToken ct)
    {
        var me = User.GetMilkmanId();
        var list = await _milkBrothers.GetMyMilkBrothersAsync(me, ct);
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var m = await _milkmen.GetByIdAsync(id, ct);
        return Ok(m);
    }
}
