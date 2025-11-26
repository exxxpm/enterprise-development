using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrudControllerBase<TEntityGetDto, TEntityCreateEditDto>(
    ICrudService<TEntityGetDto, 
    TEntityCreateEditDto> service) : ControllerBase
{

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntityGetDto>>> GetAll()
    {
        var items = await service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntityGetDto>> GetById(int id)
    {
        var item = await service.GetByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TEntityGetDto>> Create([FromBody] TEntityCreateEditDto dto)
    {
        var created = await service.CreateAsync(dto);

        var idProp = typeof(TEntityGetDto).GetProperty("Id") ?? 
            throw new InvalidOperationException("DTO does not have Id property");

        var id = (int)idProp.GetValue(created)!;

        return CreatedAtAction(nameof(GetById), new { id }, created);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TEntityGetDto>> Update(int id, [FromBody] TEntityCreateEditDto dto)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(id);
        return NoContent();
    }
}
