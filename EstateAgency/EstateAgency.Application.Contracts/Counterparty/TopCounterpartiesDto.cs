namespace EstateAgency.Application.Contracts.Counterparty;

/// <summary>
/// Data transfer object representing the top counterparties by number of purchase and sale applications.
/// </summary>
/// <param name="TopPurchase">List of top counterparties for purchase applications.</param>
/// <param name="TopSale">List of top counterparties for sale applications.</param>
public record TopCounterpartiesDto(
    List<TopCounterpartyDto> TopPurchase,
    List<TopCounterpartyDto> TopSale
);
