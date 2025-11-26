namespace EstateAgency.Application.Contracts.Property;

/// <summary>
/// Data transfer object representing the count of applications grouped by property type.
/// </summary>
/// <param name="PropertyType">The type of the property (e.g., Apartment, House).</param>
/// <param name="Count">Number of applications associated with this property type.</param>
public record PropertyTypeCountDto(
    string PropertyType,
    int Count
);
