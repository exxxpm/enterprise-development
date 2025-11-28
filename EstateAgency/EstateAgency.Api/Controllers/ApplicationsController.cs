using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Controller for managing applications.
/// Inherits generic CRUD operations from CrudControllerBase, with custom error handling for Create and Update.
/// </summary>
[ApiController]
[Route("api/application")]
public class ApplicationsController(ICrudService<ApplicationGetDto, ApplicationCreateEditDto> service) 
    : CrudControllerBase<ApplicationGetDto, ApplicationCreateEditDto>(service)
{
    /// <summary>
    /// Creates a new application with validation and custom error handling.
    /// </summary>
    /// <param name="dto">The DTO containing application data.</param>
    /// <returns>The created application DTO or a BadRequest with error details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<ApplicationGetDto>> Create([FromBody] ApplicationCreateEditDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);      
        }
    }

    /// <summary>
    /// Updates an existing application with validation and custom error handling.
    /// </summary>
    /// <param name="id">The ID of the application to update.</param>
    /// <param name="dto">The DTO containing updated application data.</param>
    /// <returns>The updated application DTO or a BadRequest with error details.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<ApplicationGetDto>> Update(int id, [FromBody] ApplicationCreateEditDto dto)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
