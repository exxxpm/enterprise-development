using EstateAgency.Application.Contracts.Interfaces;
using EstateAgency.Application.Contracts.Property;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Controller for managing properties.
/// Inherits generic CRUD operations from CrudControllerBase, with custom error handling for Create and Update.
/// </summary>
[ApiController]
[Route("api/property")]
public class PropertiesController(ICrudService<PropertyGetDto, PropertyCreateEditDto> service)
    : CrudControllerBase<PropertyGetDto, PropertyCreateEditDto>(service)
{
    /// <summary>
    /// Creates a new property with validation.
    /// </summary>
    /// <param name="dto">The DTO containing property data.</param>
    /// <returns>The created property DTO or a BadRequest with error details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PropertyGetDto>> Create([FromBody] PropertyCreateEditDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing property with validation.
    /// </summary>
    /// <param name="id">The ID of the property to update.</param>
    /// <param name="dto">The DTO containing updated property data.</param>
    /// <returns>The updated property DTO or a BadRequest/NotFound with error details.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PropertyGetDto>> Update(int id, [FromBody] PropertyCreateEditDto dto)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
