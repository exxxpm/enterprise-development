using AutoMapper;
using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;
using EstateAgency.Domain;
using EstateAgency.Domain.Entitites;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Application.Services;

/// <summary>
/// Service providing analytics on applications, counterparties, and properties.
/// </summary>
/// <param name="applicationRepo">Repository for accessing application data.</param>
/// <param name="counterpartyRepo">Repository for accessing counterparty data.</param>
/// <param name="propertyRepo">Repository for accessing property data.</param>
/// <param name="mapper">AutoMapper instance for mapping entities to DTOs.</param>
public class AnalyticService(
    IRepository<Domain.Entitites.Application> applicationRepo,
    IRepository<Counterparty> counterpartyRepo,
    IRepository<Property> propertyRepo,
    IMapper mapper)
{
    /// <summary>
    /// Retrieves counterparties who sold properties within a specified period.
    /// </summary>
    /// <param name="startDate">Start date of the period.</param>
    /// <param name="endDate">End date of the period.</param>
    /// <returns>List of counterparties as DTOs.</returns>
    public async Task<List<CounterpartyGetDto>> CouterpatriesByPeriodAsync(DateOnly startDate, DateOnly endDate)
    {
        var applications = await applicationRepo.GetAllAsync();
        var counterparties = await counterpartyRepo.GetAllAsync();

        var sellers = applications
            .Where(a => a.Type == ApplicationType.Sale && a.CreatedAt >= startDate && a.CreatedAt <= endDate)
            .Select(a => counterparties.First(c => c.Id == a.CounterpartyId))
            .Distinct()
            .ToList();

        return mapper.Map<List<CounterpartyGetDto>>(sellers);
    }

    /// <summary>
    /// Retrieves the top 5 counterparties by number of purchase and sale applications.
    /// </summary>
    /// <returns>A DTO containing top counterparties for purchases and sales.</returns>
    public async Task<TopCounterpartiesDto> TopCounterpartiesAsync()
    {
        var applications = await applicationRepo.GetAllAsync();
        var counterparties = await counterpartyRepo.GetAllAsync();
        var topCount = 5;

        var topPurchase = applications
            .Where(a => a.Type == ApplicationType.Purchase)
            .GroupBy(a => a.CounterpartyId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => new TopCounterpartyDto(
                Client: mapper.Map<CounterpartyGetDto>(counterparties.First(c => c.Id == g.Key)),
                ApplicationCount: g.Count()
            ))
            .ToList();

        var topSale = applications
            .Where(a => a.Type == ApplicationType.Sale)
            .GroupBy(a => a.CounterpartyId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => new TopCounterpartyDto(
                Client: mapper.Map<CounterpartyGetDto>(counterparties.First(c => c.Id == g.Key)),
                ApplicationCount: g.Count()
            ))
            .ToList();

        return new TopCounterpartiesDto(topPurchase, topSale);
    }

    /// <summary>
    /// Counts applications grouped by property type.
    /// </summary>
    /// <returns>List of DTOs containing property type and application count.</returns>
    public async Task<List<PropertyTypeCountDto>> PropertyTypeCountAsync()
    {
        var applications = await applicationRepo.GetAllAsync();
        var properties = await propertyRepo.GetAllAsync();

        var counts = applications
            .GroupBy(a => properties.First(p => p.Id == a.PropertyId).Type)
            .Select(g => new PropertyTypeCountDto(
                PropertyType: g.Key.ToString(),
                Count: g.Count()
            ))
            .ToList();

        return counts;
    }

    /// <summary>
    /// Retrieves counterparties with the minimum total application cost.
    /// </summary>
    /// <returns>List of DTOs containing counterparties and their minimum application cost.</returns>
    public async Task<List<ClientWithMinRequestDto>> MinPriceCounterpartiesAsync()
    {
        var applications = await applicationRepo.GetAllAsync();
        var counterparties = await counterpartyRepo.GetAllAsync();

        var minCost = applications.Min(a => a.TotalCost);

        var clients = applications
            .Where(a => a.TotalCost == minCost)
            .Select(a => counterparties.First(c => c.Id == a.CounterpartyId))
            .Distinct()
            .Select(c => new ClientWithMinRequestDto(
                mapper.Map<CounterpartyGetDto>(c),
                minCost
            ))
            .ToList();

        return clients;
    }

    /// <summary>
    /// Retrieves counterparties who own properties of a specific type.
    /// </summary>
    /// <param name="propertyType">The type of property to filter by.</param>
    /// <returns>List of counterparties as DTOs.</returns>
    public async Task<List<CounterpartyGetDto>> PropertyTypeCountAsync(PropertyType propertyType)
    {
        var applications = await applicationRepo.GetAllAsync();
        var counterparties = await counterpartyRepo.GetAllAsync();
        var properties = await propertyRepo.GetAllAsync();

        var clients = applications
            .Where(a => properties.First(p => p.Id == a.PropertyId).Type == propertyType)
            .Select(a => counterparties.First(c => c.Id == a.CounterpartyId))
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        return mapper.Map<List<CounterpartyGetDto>>(clients);
    }
}
