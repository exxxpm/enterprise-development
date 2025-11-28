using EstateAgency.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgency.Domain.Entitites;

/// <summary>
/// Represents an application record linked to a counterparty and a property.
/// </summary>
[Table("applications")]
public class Application
{
    /// <summary>
    /// Unique identifier of the application.
    /// </summary>
    [Key]
    [Column("Id")]
    public required int Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated counterparty.
    /// </summary>
    [ForeignKey(nameof(Counterparty))]
    [Column("CounterpartyId")]
    public required int CounterpartyId { get; set; }

    /// <summary>
    /// Foreign key referencing the associated property.
    /// </summary>
    [ForeignKey(nameof(Property))]
    [Column("PropertyId")]
    public required int PropertyId { get; set; }

    /// <summary>
    /// Type of the application.
    /// </summary>
    [Column("Type")]
    public required ApplicationType Type { get; set; }

    /// <summary>
    /// Total cost associated with the application.
    /// </summary>
    [Column("TotalCost")]
    public required int TotalCost { get; set; }

    /// <summary>
    /// Date when the application was created.
    /// </summary>
    [Column("CreatedAt")]
    public required DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// Navigation property to the associated counterparty.
    /// </summary>
    public Counterparty? Counterparty { get; set; }

    /// <summary>
    /// Navigation property to the associated property.
    /// </summary>
    public Property? Property { get; set; }
}
