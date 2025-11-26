using EstateAgency.Application.Contracts.Application;
using EstateAgency.Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/application")]
public class ApplicationsController(ICrudService<ApplicationGetDto, ApplicationCreateEditDto> service) 
    : CrudControllerBase<ApplicationGetDto, ApplicationCreateEditDto>(service)
{
    [HttpPost]
    public override async Task<ActionResult<ApplicationGetDto>> Create([FromBody] ApplicationCreateEditDto dto)
    {
        try
        {
            var created = await service.CreateAsync(dto);

            var idProp = typeof(ApplicationGetDto).GetProperty("Id") ?? 
                throw new InvalidOperationException("DTO does not have Id property");

            var id = (int)idProp.GetValue(created)!;

            return CreatedAtAction(nameof(GetById), new { id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPut("{id}")]
    public override async Task<ActionResult<ApplicationGetDto>> Update(int id, [FromBody] ApplicationCreateEditDto dto)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
