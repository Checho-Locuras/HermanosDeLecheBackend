using HermanosDeLeche.Api.Security;
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
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var list = await _cows.ListAsync(ct);
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var cow = await _cows.GetByIdAsync(id, ct);
        return Ok(cow);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCowRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var milkmanId = User.GetMilkmanId();
        var created = await _cows.CreateAsync(milkmanId, request, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCowRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var milkmanId = User.GetMilkmanId();
        var updated = await _cows.UpdateAsync(milkmanId, id, request, ct);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var milkmanId = User.GetMilkmanId();
        await _cows.DeleteAsync(milkmanId, id, ct);
        return NoContent();
    }
}
