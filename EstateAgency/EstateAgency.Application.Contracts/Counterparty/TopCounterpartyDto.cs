namespace EstateAgency.Application.Contracts.Counterparty;

/// <summary>
/// Data transfer object representing a counterparty along with their total number of applications.
/// </summary>
/// <param name="Client">The counterparty details.</param>
/// <param name="ApplicationCount">The number of applications associated with the counterparty.</param>
public record TopCounterpartyDto(
    CounterpartyGetDto Client,
    int ApplicationCount
);
