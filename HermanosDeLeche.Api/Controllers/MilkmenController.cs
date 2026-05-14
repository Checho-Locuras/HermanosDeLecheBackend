using HermanosDeLeche.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HermanosDeLeche.Api.Controllers;

[ApiController]
[Route("api/milkmen")]
[AllowAnonymous]
public sealed class MilkmenController : ControllerBase
{
    private readonly IMilkmanService _milkmen;

    public MilkmenController(IMilkmanService milkmen)
    {
        _milkmen = milkmen;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _milkmen.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var m = await _milkmen.GetByIdAsync(id, ct);
        return Ok(m);
    }
}
