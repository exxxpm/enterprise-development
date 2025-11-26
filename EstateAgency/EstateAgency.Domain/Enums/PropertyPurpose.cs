namespace EstateAgency.Domain.Enums;

/// <summary>
/// Defines the intended purpose of a property.
/// </summary>
public enum PropertyPurpose
{
    /// <summary>
    /// Property is intended for residential use.
    /// </summary>
    Residential,

    /// <summary>
    /// Property is intended for commercial use.
    /// </summary>
    Commercial,

    /// <summary>
    /// Property is a plot of land.
    /// </summary>
    Land,

    /// <summary>
    /// Property has a special purpose.
    /// </summary>
    Special,

    /// <summary>
    /// Property purpose does not fall into other categories.
    /// </summary>
    Other
}
