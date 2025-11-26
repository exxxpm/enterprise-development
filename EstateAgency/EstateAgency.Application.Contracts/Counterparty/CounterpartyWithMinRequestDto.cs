namespace EstateAgency.Application.Contracts.Counterparty;

public record ClientWithMinRequestDto(
    CounterpartyGetDto Client,
    int MinRequestTotalCost
);
