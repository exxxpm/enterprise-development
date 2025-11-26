using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;

namespace EstateAgency.Application.Contracts.Application;

public record ApplicationGetDto(
    int Id,
    int CounterpartyId,
    int PropertyId,
    string Type,
    int TotalCost,
    DateOnly CreatedAt,
    PropertyGetDto Property,
    CounterpartyGetDto Counterparty
);
