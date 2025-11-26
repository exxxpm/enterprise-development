namespace EstateAgency.Application.Contracts.Property;

/// <summary>
/// Data transfer object used for creating or editing a property.
/// </summary>
/// <param name="CadastralNumber">Cadastral number of the property.</param>
/// <param name="Type">Type of the property (e.g., Apartment, House).</param>
/// <param name="Purpose">Purpose of the property (e.g., Residential, Commercial).</param>
/// <param name="Address">Physical address of the property.</param>
/// <param name="TotalFloors">Total number of floors in the property.</param>
/// <param name="TotalArea">Total area of the property in square meters.</param>
/// <param name="RoomCount">Number of rooms in the property.</param>
/// <param name="CeilingHeight">Height of the ceilings in meters.</param>
/// <param name="Floor">Floor number on which the property is located.</param>
/// <param name="HasEncumbrances">Indicates whether the property has encumbrances.</param>
public record PropertyCreateEditDto(
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
