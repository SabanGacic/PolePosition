using Microsoft.AspNetCore.Mvc;
using PolePosition.API.Common;
using PolePosition.API.DTOs;
using PolePosition.API.Services;

namespace PolePosition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TracksController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TrackDto>>> Get([FromQuery] TrackSearchQuery query)
        => Ok(await _trackService.GetAsync(query));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrackDto>> GetById(int id)
        => Ok(await _trackService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<TrackDto>> Create(TrackUpsertDto dto)
    {
        var created = await _trackService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TrackDto>> Update(int id, TrackUpsertDto dto)
        => Ok(await _trackService.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _trackService.DeleteAsync(id);
        return NoContent();
    }
}