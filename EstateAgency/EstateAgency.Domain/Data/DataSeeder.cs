using EstateAgency.Domain.Entitites;
using EstateAgency.Domain.Enums;
using System.Collections.Generic;

namespace EstateAgency.Domain.Data;

/// <summary>
/// Provides seed data for testing or initializing the database, including counterparties, properties, and applications.
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Predefined list of counterparties for seeding.
    /// </summary>
    public readonly List<Counterparty> Counterparties =
    [
        new() 
        { 
            Id = 1, 
            FullName = "John Smith", 
            PassportNumber = "AB1234567", 
            PhoneNumber = "+15550000001" 
        },
        new() 
        { 
            Id = 2, 
            FullName = "Emily Johnson", 
            PassportNumber = "AB2234567", 
            PhoneNumber = "+15550000002" 
        },
        new() 
        { 
            Id = 3, 
            FullName = "Michael Brown", 
            PassportNumber = "AB3234567", 
            PhoneNumber = "+15550000003" 
        },
        new() 
        { 
            Id = 4, 
            FullName = "Sarah Davis", 
            PassportNumber = "AB4234567", 
            PhoneNumber = "+15550000004" 
        },
        new() 
        { 
            Id = 5, 
            FullName = "David Wilson", 
            PassportNumber = "AB5234567", 
            PhoneNumber = "+15550000005" 
        },
        new() 
        { 
            Id = 6, 
            FullName = "Olivia Miller", 
            PassportNumber = "AB6234567", 
            PhoneNumber = "+15550000006" 
        },
        new() 
        { 
            Id = 7, 
            FullName = "James Taylor", 
            PassportNumber = "AB7234567", 
            PhoneNumber = "+15550000007" 
        },
        new() 
        { 
            Id = 8, 
            FullName = "Sophia Anderson", 
            PassportNumber = "AB8234567", 
            PhoneNumber = "+15550000008" 
        },
        new() 
        { 
            Id = 9, 
            FullName = "Benjamin Thomas", 
            PassportNumber = "AB9234567", 
            PhoneNumber = "+15550000009" 
        },
        new() 
        { 
            Id = 10, 
            FullName = "Charlotte White", 
            PassportNumber = "AB1034567", 
            PhoneNumber = "+15550000010" 
        }
    ];

    /// <summary>
    /// Predefined list of properties for seeding.
    /// </summary>
    public readonly List<Property> Properties =
    [
        new()
        {
            Id = 1,
            CadastralNumber = "01:01:001:0001",
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            Address = "12 Main St",
            TotalFloors = 10,
            TotalArea = 54.5m,
            RoomCount = 2,
            CeilingHeight = 2.7m,
            Floor = 4,
            HasEncumbrances = false
        },
        new()
        {
            Id = 2,
            CadastralNumber = "01:01:001:0002",
            Type = PropertyType.House,
            Purpose = PropertyPurpose.Residential,
            Address = "45 Oak Avenue",
            TotalFloors = 2,
            TotalArea = 120.0m,
            RoomCount = 5,
            CeilingHeight = 3.0m,
            Floor = 1,
            HasEncumbrances = true
        },
        new()
        {
            Id = 3,
            CadastralNumber = "01:01:001:0003",
            Type = PropertyType.Townhouse,
            Purpose = PropertyPurpose.Residential,
            Address = "88 Pine Road",
            TotalFloors = 3,
            TotalArea = 98.2m,
            RoomCount = 4,
            CeilingHeight = 2.8m,
            Floor = 1,
            HasEncumbrances = false
        },
        new()
        {
            Id = 4,
            CadastralNumber = "01:01:001:0004",
            Type = PropertyType.Office,
            Purpose = PropertyPurpose.Commercial,
            Address = "200 Business St",
            TotalFloors = 15,
            TotalArea = 75.0m,
            RoomCount = 3,
            CeilingHeight = 2.9m,
            Floor = 7,
            HasEncumbrances = false
        },
        new()
        {
            Id = 5,
            CadastralNumber = "01:01:001:0005",
            Type = PropertyType.Warehouse,
            Purpose = PropertyPurpose.Commercial,
            Address = "5 Industrial Zone",
            TotalFloors = 1,
            TotalArea = 350.0m,
            RoomCount = 1,
            CeilingHeight = 5.0m,
            Floor = 1,
            HasEncumbrances = true
        },
        new()
        {
            Id = 6,
            CadastralNumber = "01:01:001:0006",
            Type = PropertyType.Apartment,
            Purpose = PropertyPurpose.Residential,
            Address = "99 Lakeview Blvd",
            TotalFloors = 12,
            TotalArea = 43.1m,
            RoomCount = 1,
            CeilingHeight = 2.6m,
            Floor = 10,
            HasEncumbrances = false
        },
        new()
        {
            Id = 7,
            CadastralNumber = "01:01:001:0007",
            Type = PropertyType.Office,
            Purpose = PropertyPurpose.Commercial,
            Address = "321 Market Street",
            TotalFloors = 20,
            TotalArea = 110.0m,
            RoomCount = 4,
            CeilingHeight = 3.1m,
            Floor = 9,
            HasEncumbrances = false
        },
        new()
        {
            Id = 8,
            CadastralNumber = "01:01:001:0008",
            Type = PropertyType.House,
            Purpose = PropertyPurpose.Residential,
            Address = "71 Sunset Drive",
            TotalFloors = 1,
            TotalArea = 80.0m,
            RoomCount = 3,
            CeilingHeight = 2.9m,
            Floor = 1,
            HasEncumbrances = false
        },
        new()
        {
            Id = 9,
            CadastralNumber = "01:01:001:0009",
            Type = PropertyType.Warehouse,
            Purpose = PropertyPurpose.Commercial,
            Address = "77 Logistics Park",
            TotalFloors = 1,
            TotalArea = 500.0m,
            RoomCount = 1,
            CeilingHeight = 6.0m,
            Floor = 1,
            HasEncumbrances = false
        },
        new()
        {
            Id = 10,
            CadastralNumber = "01:01:001:0010",
            Type = PropertyType.Other,
            Purpose = PropertyPurpose.Other,
            Address = "Unknown Facility",
            TotalFloors = 1,
            TotalArea = 65.0m,
            RoomCount = 2,
            CeilingHeight = 2.5m,
            Floor = 1,
            HasEncumbrances = false
        }
    ];

    /// <summary>
    /// Predefined list of applications for seeding.
    /// </summary>
    public readonly List<Application> Applications =
    [
        new() 
        { 
            Id = 1,
            CounterpartyId = 1,
            PropertyId = 1, 
            Type = ApplicationType.Purchase,
            TotalCost = 50000,
            CreatedAt = new DateOnly(2025, 1, 5)
        },
        new() 
        { 
            Id = 2, 
            CounterpartyId = 2, 
            PropertyId = 2, 
            Type = ApplicationType.Sale, 
            TotalCost = 180000,
            CreatedAt = new DateOnly(2025, 2, 10)
        },
        new() 
        { 
            Id = 3, 
            CounterpartyId = 3, 
            PropertyId = 3, 
            Type = ApplicationType.Purchase, 
            TotalCost = 95000,
            CreatedAt = new DateOnly(2025, 3, 15)
        },
        new() 
        { 
            Id = 4, CounterpartyId = 4,
            PropertyId = 4, 
            Type = ApplicationType.Purchase, 
            TotalCost = 220000,
            CreatedAt = new DateOnly(2025, 4, 20)
        },
        new() 
        { 
            Id = 5, 
            CounterpartyId = 5, 
            PropertyId = 5,
            Type = ApplicationType.Sale,
            TotalCost = 300000,
            CreatedAt = new DateOnly(2025, 5, 25)
        },
        new()
        { 
            Id = 6, 
            CounterpartyId = 6, 
            PropertyId = 6, 
            Type = ApplicationType.Purchase, 
            TotalCost = 70000,
            CreatedAt = new DateOnly(2025, 6, 30)
        },
        new() 
        { 
            Id = 7, 
            CounterpartyId = 7, 
            PropertyId = 7, 
            Type = ApplicationType.Sale,
            TotalCost = 260000,
            CreatedAt = new DateOnly(2025, 7, 5)
        },
        new() 
        { 
            Id = 8, 
            CounterpartyId = 8,
            PropertyId = 8,
            Type = ApplicationType.Purchase,
            TotalCost = 120000,
            CreatedAt = new DateOnly(2025, 8, 10)
        },
        new()
        { 
            Id = 9, 
            CounterpartyId = 9, 
            PropertyId = 9, 
            Type = ApplicationType.Sale,
            TotalCost = 450000,
            CreatedAt = new DateOnly(2025, 9, 15)
        },
        new()
        { 
            Id = 10, 
            CounterpartyId = 10,
            PropertyId = 10, 
            Type = ApplicationType.Purchase,
            TotalCost = 85000,
            CreatedAt = new DateOnly(2025, 10, 20)
        }
    ];
}
