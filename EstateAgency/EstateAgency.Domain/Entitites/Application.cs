using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Entitites;
public class Application
{
    public required int Id { get; set; }
    public required int CounterpartyId { get; set; }
    public required int PropertyId { get; set; }
    public required ApplicationType Type { get; set; }
    public required int TotalCost { get; set; }
    
    public virtual Counterparty? Counterparty { get; set; }
    public virtual Property? Property { get; set; }
}
