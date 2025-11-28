using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Contracts.Interfaces;

/// <summary>
/// Service providing analytics on applications, counterparties, and properties.
/// </summary>
public interface IAnalyticService
{
    /// <summary>
    /// Retrieves counterparties who sold properties within a specified period.
    /// </summary>
    /// <param name="startDate">Start date of the period.</param>
    /// <param name="endDate">End date of the period.</param>
    /// <returns>List of counterparties as DTOs.</returns>
    public Task<List<CounterpartyGetDto>> CouterpatriesByPeriodAsync(DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Retrieves the top 5 counterparties by number of purchase and sale applications.
    /// </summary>
    /// <returns>A DTO containing top counterparties for purchases and sales.</returns>
    public Task<TopCounterpartiesDto> TopCounterpartiesAsync();

    /// <summary>
    /// Counts applications grouped by property type.
    /// </summary>
    /// <returns>List of DTOs containing property type and application count.</returns>
    public Task<List<PropertyTypeCountDto>> PropertyTypeCountAsync();

    /// <summary>
    /// Retrieves counterparties with the minimum total application cost.
    /// </summary>
    /// <returns>List of DTOs containing counterparties and their minimum application cost.</returns>
    public Task<List<ClientWithMinRequestDto>> MinPriceCounterpartiesAsync();

    /// <summary>
    /// Retrieves counterparties who own properties of a specific type.
    /// </summary>
    /// <param name="propertyType">The type of property to filter by.</param>
    /// <returns>List of counterparties as DTOs.</returns>
    public Task<List<CounterpartyGetDto>> PropertyTypeCountAsync(PropertyType propertyType);
}

