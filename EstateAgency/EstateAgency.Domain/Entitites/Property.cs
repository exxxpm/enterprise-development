using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Entitites;

public class Property
{
    public required int Id { get; set; }
    public required string CadastralNumber { get; set; }
    public required PropertyType Type { get; set; }
    public required PropertyPurpose Purpose { get; set; }
    public required string Address { get; set; }
    public required int TotalFloors { get; set; }
    public required decimal TotalArea { get; set; }
    public required int RoomCount { get; set; }
    public required decimal CeilingHeight { get; set; }
    public required int Floor { get; set; }
    public required bool HasEncumbrances { get; set; }
}
