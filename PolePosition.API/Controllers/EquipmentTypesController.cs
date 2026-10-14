using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolePosition.API.Common;
using PolePosition.API.DTOs;
using PolePosition.API.Services;

namespace PolePosition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentTypesController : ControllerBase
{
    private readonly IEquipmentTypeService _equipmentTypeService;

    public EquipmentTypesController(IEquipmentTypeService equipmentTypeService)
    {
        _equipmentTypeService = equipmentTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<EquipmentTypeDto>>> Get([FromQuery] EquipmentTypeSearchQuery query)
        => Ok(await _equipmentTypeService.GetAsync(query));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EquipmentTypeDto>> GetById(int id)
        => Ok(await _equipmentTypeService.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<EquipmentTypeDto>> Create(EquipmentTypeUpsertDto dto)
    {
        var created = await _equipmentTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<EquipmentTypeDto>> Update(int id, EquipmentTypeUpsertDto dto)
        => Ok(await _equipmentTypeService.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        await _equipmentTypeService.DeleteAsync(id);
        return NoContent();
    }
}