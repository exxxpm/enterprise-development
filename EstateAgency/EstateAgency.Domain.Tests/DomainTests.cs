using EstateAgency.Domain.Data;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Tests;

/// <summary>
/// Unit tests for domain logic using seeded data, verifying application queries, client statistics, and property-related aggregations.
/// </summary>
public class DomainTests(DataSeeder data) : IClassFixture<DataSeeder>
{
    /// <summary>
    /// Tests retrieval of counterparties who have sale applications within a specified period.
    /// </summary>
    [Fact]
    public void GetSellersByPeriod()
    {
        const int expectedCount = 2;

        var startDate = new DateOnly(2025, 7, 1);
        var endDate = new DateOnly(2025, 12, 31);

        var sellers = data.Applications
            .Where(a => a.Type == ApplicationType.Sale &&
                a.CreatedAt >= startDate &&
                a.CreatedAt <= endDate)
            .Join(
                data.Counterparties,
                a => a.CounterpartyId,
                c => c.Id,
                (a, c) => c
            )
            .Distinct()
            .ToList();

        Assert.Equal(expectedCount, sellers.Count);
    }

    /// <summary>
    /// Tests retrieval of top 5 clients by number of purchase and sale applications.
    /// </summary>
    [Fact]
    public void Top5ClientsByApplicationCount()
    {
        const int topCount = 5;

        var expectedTopPurchaseIds = new List<int> { 1, 3, 4, 6, 8 };
        var expectedTopSaleIds = new List<int> { 2, 5, 7, 9 };

        var actualTopPurchaseIds = data.Applications
            .Where(a => a.Type == ApplicationType.Purchase)
            .GroupBy(a => a.CounterpartyId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.Key) 
            .ToList();

        var actualTopSaleIds = data.Applications
            .Where(a => a.Type == ApplicationType.Sale)
            .GroupBy(a => a.CounterpartyId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.Key)
            .ToList();

        Assert.Equal(expectedTopPurchaseIds, actualTopPurchaseIds);
        Assert.Equal(expectedTopSaleIds, actualTopSaleIds);
    }

    /// <summary>
    /// Tests counting of applications grouped by property type and compares with expected values.
    /// </summary>
    [Fact]
    public void ApplicationCountByPropertyType()
    {
        var expectedCounts = new Dictionary<PropertyType, int>
        {
            { PropertyType.Apartment, 2 },
            { PropertyType.House, 2 },
            { PropertyType.Townhouse, 1 },
            { PropertyType.Office, 2 },
            { PropertyType.Warehouse, 2 },
            { PropertyType.Other, 1 }
        };

        var counts = data.Applications
            .Join(
                data.Properties,
                a => a.PropertyId,
                p => p.Id,
                (a, p) => new { Application = a, Property = p }
            )
            .GroupBy(x => x.Property.Type)
            .ToDictionary(
                g => g.Key,
                g => g.Count()
            );

        Assert.NotEmpty(counts);
        Assert.Equal(expectedCounts, counts);
    }

    /// <summary>
    /// Tests retrieval of counterparties with applications of a minimum specified total cost.
    /// </summary>
    [Fact]
    public void ClientsWithMinimumApplicationCost()
    {
        const int expectedMinCost = 50000;
        const int expectedClientId = 1;

        var actualMinCost = data.Applications.Min(a => a.TotalCost);
        var actualClientId = data.Applications
            .Where(a => a.TotalCost == actualMinCost)
            .Select(a => a.CounterpartyId)
            .Distinct()
            .Single();

        Assert.Equal(expectedMinCost, actualMinCost);
        Assert.Equal(expectedClientId, actualClientId);
    }

    /// <summary>
    /// Tests retrieval of counterparties associated with properties of a specific type.
    /// </summary>
    [Fact]
    public void ClientsByPropertyType()
    {
        const PropertyType targetType = PropertyType.Apartment;
        var expectedClientIds = new List<int> { 1, 6 };

        var actualClientIds = data.Applications
            .Join(
                data.Properties,
                a => a.PropertyId,
                p => p.Id,
                (a, p) => new { a.CounterpartyId, PropertyType = p.Type }
            )
            .Where(x => x.PropertyType == targetType)
            .Select(x => x.CounterpartyId)
            .Distinct()
            .OrderBy(id => id)
            .ToList();

        Assert.Equal(expectedClientIds, actualClientIds);
    }
}
