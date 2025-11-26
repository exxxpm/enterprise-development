namespace EstateAgency.Domain.Enums;

/// <summary>
/// Specifies the type or category of a property.
/// </summary>
public enum PropertyType
{
    /// <summary>
    /// Property is an apartment.
    /// </summary>
    Apartment,

    /// <summary>
    /// Property is a standalone house.
    /// </summary>
    House,

    /// <summary>
    /// Property is a townhouse.
    /// </summary>
    Townhouse,

    /// <summary>
    /// Property is a warehouse.
    /// </summary>
    Warehouse,

    /// <summary>
    /// Property is an office space.
    /// </summary>
    Office,

    /// <summary>
    /// Property type does not fall into other categories.
    /// </summary>
    Other
}
