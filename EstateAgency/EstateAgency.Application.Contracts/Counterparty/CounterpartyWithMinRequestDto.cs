namespace EstateAgency.Application.Contracts.Counterparty;

public record ClientWithMinRequestDto(
    CounterpartyGetDto Counterparty,
    int MinTotalCost
);
