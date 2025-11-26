namespace EstateAgency.Application.Contracts.Counterparty;

/// <summary>
/// Data transfer object used for creating or editing a counterparty.
/// </summary>
/// <param name="FullName">Full name of the counterparty.</param>
/// <param name="PassportNumber">Passport number of the counterparty.</param>
/// <param name="PhoneNumber">Phone number of the counterparty.</param>
public record CounterpartyCreateEditDto(
    string FullName,
    string PassportNumber,
    string PhoneNumber
);
