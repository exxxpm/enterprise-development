using EstateAgency.Domain.Data;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Tests;

public class DomainTests(DataSeeder data) : IClassFixture<DataSeeder>
{
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
