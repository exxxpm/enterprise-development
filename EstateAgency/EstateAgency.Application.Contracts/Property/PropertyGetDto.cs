namespace EstateAgency.Application.Contracts.Property;

public record PropertyGetDto(
    int Id,
    string CadastralNumber,
    string Type,
    string Purpose,
    string Address,
    int TotalFloors,
    decimal TotalArea,
    int RoomCount,
    decimal CeilingHeight,
    int Floor,
    bool HasEncumbrances
);
