using Bogus;
using EstateAgency.Application.Contracts.Application;

namespace EstateAgency.Generator;

/// <summary>
/// Utility class for generating fake <see cref="ApplicationCreateEditDto"/> objects
/// for testing or seeding purposes.
/// </summary>
public static class Generator
{
    /// <summary>
    /// Predefined list of possible application types.
    /// </summary>
    private static readonly List<string> _applicationTypes = ["Sale", "Purchase"];

    /// <summary>
    /// Creates a Faker instance for generating <see cref="ApplicationCreateEditDto"/> objects.
    /// </summary>
    /// <param name="CounterpartyIds">Maximum ID for Counterparty. Defaults to 10.</param>
    /// <param name="PropertyIds">Maximum ID for Property. Defaults to 10.</param>
    /// <returns>A <see cref="Faker{ApplicationCreateEditDto}"/> configured to generate fake applications.</returns>
    public static Faker<ApplicationCreateEditDto> Create(int counterpartyIds = 10, int propertyIds = 10) =>
        new Faker<ApplicationCreateEditDto>()
            .CustomInstantiator(f => new ApplicationCreateEditDto(
                CounterpartyId: f.Random.Int(1, counterpartyIds),
                PropertyId: f.Random.Int(1, propertyIds),
                Type: f.PickRandom(_applicationTypes),
                TotalCost: f.Random.Int(100000, 1000000)
            ));
}
