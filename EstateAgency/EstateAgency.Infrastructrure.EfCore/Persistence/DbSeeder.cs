using EstateAgency.Domain.Data;

namespace EstateAgency.Infrastructrure.EfCore.Persistence;

/// <summary>
/// Provides database seeding functionality for the Estate Agency application.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Seeds the database with initial data if the tables are empty.
    /// Resets entity IDs before adding to ensure new entries.
    /// </summary>
    /// <param name="context">The database context to seed.</param>
    /// <param name="data">The predefined seed data.</param>
    public static void Seed(EstateAgencyDbContext context, DataSeeder data)
    {
        if (context.Counterparties.Any() || context.Properties.Any() || context.Applications.Any())
            return;

        data.Counterparties.ForEach(c => c.Id = 0);
        data.Properties.ForEach(c => c.Id = 0);
        data.Applications.ForEach(c => c.Id = 0);

        context.Counterparties.AddRange(data.Counterparties);
        context.Properties.AddRange(data.Properties);
        context.SaveChanges();

        context.Applications.AddRange(data.Applications);
        context.SaveChanges();
    }
}
