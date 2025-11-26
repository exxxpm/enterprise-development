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
        var expectedCount = 2;

        var startDate = new DateOnly(2025, 7, 1);
        var endDate = new DateOnly(2025, 12, 31);

        var sellers = data.Applications
            .Where(a => a.Type == ApplicationType.Sale &&
                a.CreatedAt >= startDate &&
                a.CreatedAt <= endDate)
            .Select(a => data.Counterparties.First(c => c.Id == a.CounterpartyId))
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
        var topCount = 5;

        var top5Purchase = data.Applications
            .Where(a => a.Type == ApplicationType.Purchase)
            .GroupBy(a => a.CounterpartyId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => data.Counterparties.First(c => c.Id == g.Key))
            .ToList();

        var top5Sale = data.Applications
            .Where(a => a.Type == ApplicationType.Sale)
            .GroupBy(a => a.CounterpartyId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => data.Counterparties.First(c => c.Id == g.Key))
            .ToList();

        Assert.NotEmpty(top5Purchase);
        Assert.NotEmpty(top5Sale);
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
            .GroupBy(a => data.Properties.First(p => p.Id == a.PropertyId).Type)
            .ToDictionary(
                g => g.Key,
                g => g.Count()
            );

        Assert.NotEmpty(counts);
        foreach (var pair in expectedCounts)
        {
            Assert.Equal(pair.Value, counts[pair.Key]);
        }
    }

    /// <summary>
    /// Tests retrieval of counterparties with applications of a minimum specified total cost.
    /// </summary>
    [Fact]
    public void ClientsWithMinimumApplicationCost()
    {
        var expectedMinCost = 50000;
        var expectedClientsIds = new List<int> { 1 };

        var actualClients = data.Applications
            .Where(a => a.TotalCost == expectedMinCost)
            .Select(a => data.Counterparties.First(c => c.Id == a.CounterpartyId))
            .Distinct()
            .ToList();

        Assert.Equal(expectedClientsIds.Count, actualClients.Count);
        foreach (var client in actualClients)
        {
            Assert.Contains(client.Id, expectedClientsIds);
        }
    }

    /// <summary>
    /// Tests retrieval of counterparties associated with properties of a specific type.
    /// </summary>
    [Fact]
    public void ClientsByPropertyType()
    {
        var targetType = PropertyType.Apartment;
        var expectedClientIds = new List<int> { 1, 6 };

        var actualClients = data.Applications
            .Where(a => data.Properties.First(p => p.Id == a.PropertyId).Type == targetType)
            .Select(a => data.Counterparties.First(c => c.Id == a.CounterpartyId))
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(expectedClientIds.Count, actualClients.Count);
        foreach (var client in actualClients)
        {
            Assert.Contains(client.Id, expectedClientIds);
        }
    }
}
