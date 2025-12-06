using Bogus;
using EstateAgency.Application.Contracts.Application;

namespace EstateAgency.Generator;

public class ApplicationFaker
{
    private static readonly List<string> _applicationTypes = ["Sale", "Purchase"];

    public static Faker<ApplicationCreateEditDto> Create() =>
        new Faker<ApplicationCreateEditDto>()
            .CustomInstantiator(f => new ApplicationCreateEditDto(
                CounterpartyId: f.Random.Int(1, 1000),
                PropertyId: f.Random.Int(1, 1000),
                Type: f.PickRandom(_applicationTypes),
                TotalCost: f.Random.Int(10_000, 5_000_000)
            ));
}
