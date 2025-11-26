namespace EstateAgency.Application.Contracts.Application;

/// <summary>
/// Data transfer object used for creating or editing an application.
/// </summary>
/// <param name="CounterpartyId">Identifier of the associated counterparty.</param>
/// <param name="PropertyId">Identifier of the associated property.</param>
/// <param name="Type">Type of the application (e.g., Purchase, Sale).</param>
/// <param name="TotalCost">Total cost associated with the application.</param>
public record ApplicationCreateEditDto(
    int CounterpartyId,
    int PropertyId,
    string Type,
    int TotalCost
);
