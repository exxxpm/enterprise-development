using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Domain.Entitites;

/// <summary>
/// Represents a counterparty entity, such as a client or business partner.
/// </summary>
[Table("counterparties")]
public class Counterparty
{
    /// <summary>
    /// Unique identifier of the counterparty.
    /// </summary>
    [Key]
    [Column("Id")]
    public required int Id { get; set; }

    /// <summary>
    /// Full name of the counterparty.
    /// </summary>
    [MaxLength(100)]
    [Column("FullName")]
    public required string FullName { get; set; }

    /// <summary>
    /// Passport number of the counterparty.
    /// </summary>
    [MaxLength(20)]
    [Column("PassportNumber")]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Phone number of the counterparty.
    /// </summary>
    [Phone]
    [Column("PhoneNumber")]
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Collection of applications associated with the counterparty.
    /// </summary>
    public virtual ICollection<Application>? Applications { get; set; }
}
