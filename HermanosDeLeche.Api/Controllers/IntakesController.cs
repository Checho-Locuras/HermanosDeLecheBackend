using HermanosDeLeche.Api.Security;
using HermanosDeLeche.Domain.DTOs.Intake;
using HermanosDeLeche.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HermanosDeLeche.Api.Controllers;

[ApiController]
[Route("api/intakes")]
public sealed class IntakesController : ControllerBase
{
    private readonly IIntakeService _intakes;

    public IntakesController(IIntakeService intakes)
    {
        _intakes = intakes;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _intakes.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("cow/{cowId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> ByCow(Guid cowId, CancellationToken ct)
    {
        var list = await _intakes.ListByCowAsync(cowId, ct);
        return Ok(list);
    }

    [HttpGet("milkman/{milkmanId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> ByMilkman(Guid milkmanId, CancellationToken ct)
    {
        var list = await _intakes.ListByMilkmanAsync(milkmanId, ct);
        return Ok(list);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateIntakeRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var milkmanId = User.GetMilkmanId();
        var created = await _intakes.CreateAsync(milkmanId, request, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }
}
