using EstateAgency.Domain.Data;

namespace EstateAgency.Infrastructrure.EfCore.Persistence;

public class DbSeeder(DataSeeder data)
{
    public void Seed(EstateAgencyDbContext context)
    {
        if (context.Counterparties.Any() || context.Properties.Any() || context.Applications.Any())
            return;

        context.Counterparties.AddRange(data.Counterparties);
        context.Properties.AddRange(data.Properties);
        context.Applications.AddRange(data.Applications);

        context.SaveChanges();
    }
}
