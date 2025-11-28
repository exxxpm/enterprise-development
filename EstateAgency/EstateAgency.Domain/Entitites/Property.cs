using EstateAgency.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgency.Domain.Entitites;

/// <summary>
/// Represents a real estate property with detailed attributes and associated applications.
/// </summary>
[Table("Properties")]
public class Property
{
    /// <summary>
    /// Unique identifier of the property.
    /// </summary>
    [Key]
    [Column("Id")]
    public required int Id { get; set; }

    /// <summary>
    /// Cadastral number of the property.
    /// </summary>
    [MaxLength(50)]
    [Column("CadastralNumber")]
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Type of the property (e.g., residential, commercial).
    /// </summary>
    [Column("Type")]
    public required PropertyType Type { get; set; }

    /// <summary>
    /// Purpose of the property (e.g., sale, rent, investment).
    /// </summary>
    [Column("Purpose")]
    public required PropertyPurpose Purpose { get; set; }

    /// <summary>
    /// Physical address of the property.
    /// </summary>
    [MaxLength(200)]
    [Column("Address")]
    public required string Address { get; set; }

    /// <summary>
    /// Total number of floors in the property.
    /// </summary>
    [Column("TotalFloors")]
    public required int TotalFloors { get; set; }

    /// <summary>
    /// Total area of the property in square meters.
    /// </summary>
    [Column("TotalArea")]
    public required decimal TotalArea { get; set; }

    /// <summary>
    /// Number of rooms in the property.
    /// </summary>
    [Column("RoomCount")]
    public required int RoomCount { get; set; }

    /// <summary>
    /// Height of the ceilings in meters.
    /// </summary>
    [Column("CeilingHeight")]
    public required decimal CeilingHeight { get; set; }

    /// <summary>
    /// Floor number on which the property is located.
    /// </summary>
    [Column("Floor")]
    public required int Floor { get; set; }

    /// <summary>
    /// Indicates whether the property has encumbrances.
    /// </summary>
    [Column("HasEncumbrances")]
    public required bool HasEncumbrances { get; set; }

    /// <summary>
    /// Collection of applications associated with the property.
    /// </summary>
    public virtual ICollection<Application>? Applications { get; set; }
}
