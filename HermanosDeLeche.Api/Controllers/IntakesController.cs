using HermanosDeLeche.Api.Security;
using HermanosDeLeche.Domain.DTOs.Common;
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
    [ProducesResponseType(typeof(List<IntakeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _intakes.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("cow/{cowId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<IntakeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ByCow(Guid cowId, CancellationToken ct)
    {
        var list = await _intakes.ListByCowAsync(cowId, ct);
        return Ok(list);
    }

    [HttpGet("milkman/{milkmanId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<IntakeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ByMilkman(Guid milkmanId, CancellationToken ct)
    {
        var list = await _intakes.ListByMilkmanAsync(milkmanId, ct);
        return Ok(list);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateIntakeSuccessResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateIntakeRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var milkmanId = User.GetMilkmanId();
        var created = await _intakes.CreateAsync(milkmanId, request, ct);
        var body = new CreateIntakeSuccessResponse
        {
            Message = "Vaca lechada con éxito. Ingesta registrada.",
            Intake = created
        };
        return StatusCode(StatusCodes.Status201Created, body);
    }
}
