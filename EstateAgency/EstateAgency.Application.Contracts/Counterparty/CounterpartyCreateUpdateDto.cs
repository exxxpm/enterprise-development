namespace EstateAgency.Application.Contracts.Counterparty;

public record CounterpartyCreateEditDto(
    string FullName,
    string PassportNumber,
    string PhoneNumber
);
