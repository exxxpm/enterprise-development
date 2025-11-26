namespace EstateAgency.Application.Contracts.Application;

public record ApplicationCreateEditDto(
    int CounterpartyId,
    int PropertyId,
    string Type,
    int TotalCost
);

