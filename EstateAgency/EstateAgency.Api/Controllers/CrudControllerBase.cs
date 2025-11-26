using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Generic base controller providing standard CRUD endpoints for a given entity DTO type.
/// </summary>
/// <typeparam name="TEntityGetDto">The DTO type used for retrieval operations.</typeparam>
/// <typeparam name="TEntityCreateEditDto">The DTO type used for create and update operations.</typeparam>
/// <param name="service">The CRUD service handling business logic for the entity.</param>
[ApiController]
[Route("api/[controller]")]
public class CrudControllerBase<TEntityGetDto, TEntityCreateEditDto>(
    ICrudService<TEntityGetDto, TEntityCreateEditDto> service
) : ControllerBase
{
    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A collection of DTOs representing all entities.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IEnumerable<TEntityGetDto>>> GetAll()
    {
        var items = await service.GetAllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Retrieves a single entity by ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The DTO of the entity if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TEntityGetDto>> GetById(int id)
    {
        var item = await service.GetByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="dto">The DTO containing data for the new entity.</param>
    /// <returns>The created entity's DTO along with a 201 Created response.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<ActionResult<TEntityGetDto>> Create([FromBody] TEntityCreateEditDto dto)
    {
        var created = await service.CreateAsync(dto);

        var idProp = typeof(TEntityGetDto).GetProperty("Id") ??
            throw new InvalidOperationException("DTO does not have Id property");

        var id = (int)idProp.GetValue(created)!;

        return CreatedAtAction(nameof(GetById), new { id }, created);
    }

    /// <summary>
    /// Updates an existing entity by ID.
    /// </summary>
    /// <param name="id">The ID of the entity to update.</param>
    /// <param name="dto">The DTO containing updated data.</param>
    /// <returns>The updated entity's DTO, or NotFound if the entity does not exist.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes an entity by ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>NoContent response.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
