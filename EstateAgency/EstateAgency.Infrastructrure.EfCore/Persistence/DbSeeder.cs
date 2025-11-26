using EstateAgency.Domain.Data;

namespace EstateAgency.Infrastructrure.EfCore.Persistence;

public static class DbSeeder
{
    public static void Seed(EstateAgencyDbContext context, DataSeeder data)
    {
        if (context.Counterparties.Any() || context.Properties.Any() || context.Applications.Any())
            return;

        context.Counterparties.AddRange(data.Counterparties);
        context.Properties.AddRange(data.Properties);
        context.Applications.AddRange(data.Applications);

        context.SaveChanges();
    }
}
