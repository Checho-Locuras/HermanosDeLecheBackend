using HermanosDeLeche.Api.Security;
using HermanosDeLeche.Domain.DTOs.Common;
using HermanosDeLeche.Domain.DTOs.Cow;
using HermanosDeLeche.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HermanosDeLeche.Api.Controllers;

[ApiController]
[Route("api/cows")]
public sealed class CowsController : ControllerBase
{
    private readonly ICowService _cows;

    public CowsController(ICowService cows)
    {
        _cows = cows;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<CowResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _cows.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CowResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var cow = await _cows.GetByIdAsync(id, ct);
        return Ok(cow);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CowMutationSuccessResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateCowRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var milkmanId = User.GetMilkmanId();
        var created = await _cows.CreateAsync(milkmanId, request, ct);
        var body = new CowMutationSuccessResponse
        {
            Message = "Vaca registrada con éxito.",
            Cow = created
        };
        return CreatedAtAction(nameof(Get), new { id = created.Id }, body);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CowMutationSuccessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCowRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var milkmanId = User.GetMilkmanId();
        var updated = await _cows.UpdateAsync(milkmanId, id, request, ct);
        return Ok(new CowMutationSuccessResponse
        {
            Message = "Vaca actualizada con éxito.",
            Cow = updated
        });
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiMessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var milkmanId = User.GetMilkmanId();
        await _cows.DeleteAsync(milkmanId, id, ct);
        return Ok(new ApiMessageResponse { Message = "Vaca eliminada con éxito." });
    }
}
