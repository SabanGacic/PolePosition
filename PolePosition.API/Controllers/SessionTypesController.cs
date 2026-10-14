using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolePosition.API.Common;
using PolePosition.API.DTOs;
using PolePosition.API.Services;

namespace PolePosition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionTypesController : ControllerBase
{
    private readonly ISessionTypeService _sessionTypeService;

    public SessionTypesController(ISessionTypeService sessionTypeService)
    {
        _sessionTypeService = sessionTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SessionTypeDto>>> Get([FromQuery] SessionTypeSearchQuery query)
        => Ok(await _sessionTypeService.GetAsync(query));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SessionTypeDto>> GetById(int id)
        => Ok(await _sessionTypeService.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<SessionTypeDto>> Create(SessionTypeUpsertDto dto)
    {
        var created = await _sessionTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<SessionTypeDto>> Update(int id, SessionTypeUpsertDto dto)
        => Ok(await _sessionTypeService.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        await _sessionTypeService.DeleteAsync(id);
        return NoContent();
    }
}