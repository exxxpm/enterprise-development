namespace EstateAgency.Application.Contracts.Counterparty;

public record CounterpartyGetDto(
    int Id,
    string FullName,
    string PassportNumber,
    string PhoneNumber
);
