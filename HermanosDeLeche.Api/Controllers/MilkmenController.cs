using HermanosDeLeche.Api.Security;
using HermanosDeLeche.Domain.DTOs.Common;
using HermanosDeLeche.Domain.DTOs.Cow;
using HermanosDeLeche.Domain.DTOs.Milkman;
using HermanosDeLeche.Domain.Exceptions;
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
    private readonly ICowService _cows;

    public MilkmenController(IMilkmanService milkmen, IMilkBrotherService milkBrothers, ICowService cows)
    {
        _milkmen = milkmen;
        _milkBrothers = milkBrothers;
        _cows = cows;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<MilkmanResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _milkmen.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("hermanos-de-leche")]
    [Authorize]
    [ProducesResponseType(typeof(List<MilkBrotherResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MyMilkBrothers(CancellationToken ct)
    {
        var me = User.GetMilkmanId();
        var list = await _milkBrothers.GetMyMilkBrothersAsync(me, ct);
        return Ok(list);
    }

    [HttpGet("cows/{idUser:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(List<CowResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CowsFedByMilkman(Guid idUser, CancellationToken ct)
    {
        if (idUser != User.GetMilkmanId())
            throw new AppException("Solo puedes consultar tus propias vacas lechadas.", 403);

        var list = await _cows.ListFedByMilkmanAsync(idUser, ct);
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(MilkmanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var m = await _milkmen.GetByIdAsync(id, ct);
        return Ok(m);
    }
}
