using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EstateAgency.Domain.Entitites;

[Table("counterparty")]
public class Counterparty
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(100)]
    [Column("full_name")]
    public required string FullName { get; set; }

    [MaxLength(20)]
    [Column("passport_number")]
    public required string PassportNumber { get; set; }

    [Phone]
    [Column("phone_number")]
    public required string PhoneNumber { get; set; }

    public virtual ICollection<Application>? Applications { get; set; }
}
