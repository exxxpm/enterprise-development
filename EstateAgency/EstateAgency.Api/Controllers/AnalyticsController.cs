using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgency.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController(AnalyticService analyticService) : ControllerBase
{
    [HttpGet("counterparties-by-period")]
    public async Task<ActionResult<List<CounterpartyGetDto>>> GetSellersByPeriod([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var sellers = await analyticService.CouterpatriesByPeriodAsync(startDate, endDate);
        return Ok(sellers);
    }

    [HttpGet("top-counterparties")]
    public async Task<ActionResult<TopCounterpartiesDto>> GetTopCounterparties()
    {
        var result = await analyticService.TopCounterpartiesAsync();
        return Ok(result);
    }

    [HttpGet("property-type-count")]
    public async Task<ActionResult<List<PropertyTypeCountDto>>> GetRequestsCountByPropertyType()
    {
        var counts = await analyticService.PropertyTypeCountAsync();
        return Ok(counts);
    }

    [HttpGet("min-price-clients")]
    public async Task<ActionResult<List<ClientWithMinRequestDto>>> GetClientsWithMinPriceRequests()
    {
        var clients = await analyticService.MinPriceCounterpartiesAsync();
        return Ok(clients);
    }

    [HttpGet("clients-by-property-type/{propertyType}")]
    public async Task<ActionResult<List<CounterpartyGetDto>>> GetClientsByPropertyType(string propertyType)
    {
        try
        {
            var clients = await analyticService.PropertyTypeCountAsync(propertyType);
            return Ok(clients);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
