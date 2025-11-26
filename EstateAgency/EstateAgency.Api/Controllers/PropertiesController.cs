using EstateAgency.Application.Contracts.Interfaces;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/property")]
public class PropertiesController(ICrudService<PropertyGetDto, PropertyCreateEditDto> service)
    : CrudControllerBase<PropertyGetDto, PropertyCreateEditDto>(service)
{
    [HttpPost]
    public override async Task<ActionResult<PropertyGetDto>> Create([FromBody] PropertyCreateEditDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPut("{id}")]
    public override async Task<ActionResult<PropertyGetDto>> Update(int id, [FromBody] PropertyCreateEditDto dto)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
