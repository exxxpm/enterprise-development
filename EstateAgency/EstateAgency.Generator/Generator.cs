using Bogus;
using EstateAgency.Application.Contracts.Application;

namespace EstateAgency.Generator;

public class Generator
{
    private static readonly List<string> _applicationTypes = ["Sale", "Purchase"];

    public static Faker<ApplicationCreateEditDto> Create() =>
        new Faker<ApplicationCreateEditDto>()
            .CustomInstantiator(f => new ApplicationCreateEditDto(
                CounterpartyId: f.Random.Int(1, 10),
                PropertyId: f.Random.Int(1, 10),
                Type: f.PickRandom(_applicationTypes),
                TotalCost: f.Random.Int(100000, 1000000)
            ));
}
