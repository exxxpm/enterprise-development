namespace EstateAgency.Application.Contracts.Counterparty;

/// <summary>
/// Data transfer object representing a counterparty along with their minimum application total cost.
/// </summary>
/// <param name="Counterparty">The counterparty details.</param>
/// <param name="MinTotalCost">The minimum total cost among the counterparty's applications.</param>
public record ClientWithMinRequestDto(
    CounterpartyGetDto Counterparty,
    int MinTotalCost
);
