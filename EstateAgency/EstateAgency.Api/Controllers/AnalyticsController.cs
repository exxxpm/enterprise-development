using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Application.Services;
using EstateAgency.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

/// <summary>
/// Controller providing analytics endpoints for applications, counterparties, and properties.
/// </summary>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticService analyticService) : ControllerBase
{
    /// <summary>
    /// Retrieves counterparties who sold properties within a specified period.
    /// </summary>
    /// <param name="startDate">Start date of the period.</param>
    /// <param name="endDate">End date of the period.</param>
    /// <returns>List of counterparties as DTOs.</returns>
    [HttpGet("counterparties-by-period")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CounterpartyGetDto>>> GetSellersByPeriod([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var sellers = await analyticService.CouterpatriesByPeriodAsync(startDate, endDate);
        return Ok(sellers);
    }

    /// <summary>
    /// Retrieves top 5 counterparties by number of purchase and sale applications.
    /// </summary>
    /// <returns>A DTO containing top counterparties for purchases and sales.</returns>
    [HttpGet("top-counterparties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TopCounterpartiesDto>> GetTopCounterparties()
    {
        var result = await analyticService.TopCounterpartiesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Counts applications grouped by property type.
    /// </summary>
    /// <returns>List of DTOs containing property type and application count.</returns>
    [HttpGet("property-type-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PropertyTypeCountDto>>> GetRequestsCountByPropertyType()
    {
        var counts = await analyticService.PropertyTypeCountAsync();
        return Ok(counts);
    }

    /// <summary>
    /// Retrieves counterparties with the minimum total application cost.
    /// </summary>
    /// <returns>List of counterparties with their minimum request cost.</returns>
    [HttpGet("min-price-clients")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientWithMinRequestDto>>> GetClientsWithMinPriceRequests()
    {
        var clients = await analyticService.MinPriceCounterpartiesAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Retrieves counterparties who own properties of a specific type.
    /// </summary>
    /// <param name="propertyType">The type of property to filter by.</param>
    /// <returns>List of counterparties as DTOs.</returns>
    [HttpGet("clients-by-property-type/{propertyType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CounterpartyGetDto>>> GetClientsByPropertyType(PropertyType propertyType)
    {
        var clients = await analyticService.PropertyTypeCountAsync(propertyType);
        return Ok(clients);
    }
}
