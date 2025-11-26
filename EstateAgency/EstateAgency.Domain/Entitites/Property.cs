using EstateAgency.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgency.Domain.Entitites;

[Table("properties")]
public class Property
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [MaxLength(50)]
    [Column("cadastral_number")]
    public required string CadastralNumber { get; set; }

    [Column("type")]
    public required PropertyType Type { get; set; }

    [Column("purpose")]
    public required PropertyPurpose Purpose { get; set; }

    [MaxLength(200)]
    [Column("address")]
    public required string Address { get; set; }

    [Column("total_floors")]
    public required int TotalFloors { get; set; }

    [Column("total_area")]
    public required decimal TotalArea { get; set; }

    [Column("room_count")]
    public required int RoomCount { get; set; }

    [Column("ceiling_height")]
    public required decimal CeilingHeight { get; set; }

    [Column("floor")]
    public required int Floor { get; set; }

    [Column("has_encumbrances")]
    public required bool HasEncumbrances { get; set; }

    public virtual ICollection<Application>? Applications { get; set; }
}
