using EstateAgency.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgency.Domain.Entitites;

[Table("applications")]
public class Application
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [ForeignKey(nameof(Counterparty))]
    [Column("counterparty_id")]
    public required int CounterpartyId { get; set; }

    [ForeignKey(nameof(Property))]
    [Column("property_id")]
    public required int PropertyId { get; set; }

    [Column("type")]
    public required ApplicationType Type { get; set; }

    [Column("total_cost")]
    public required int TotalCost { get; set; }

    [Column("created_at")]
    public required DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public virtual Counterparty? Counterparty { get; set; }

    public virtual Property? Property { get; set; }
}
