using EstateAgency.Application.Contracts.Counterparty;
using EstateAgency.Application.Contracts.Property;

namespace EstateAgency.Application.Contracts.Application;

/// <summary>
/// Data transfer object representing an application for retrieval operations.
/// </summary>
/// <param name="Id">Unique identifier of the application.</param>
/// <param name="CounterpartyId">Identifier of the associated counterparty.</param>
/// <param name="PropertyId">Identifier of the associated property.</param>
/// <param name="Type">Type of the application (e.g., Purchase, Sale).</param>
/// <param name="TotalCost">Total cost associated with the application.</param>
/// <param name="CreatedAt">Date when the application was created.</param>
/// <param name="Property">Detailed information about the associated property.</param>
/// <param name="Counterparty">Detailed information about the associated counterparty.</param>
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
