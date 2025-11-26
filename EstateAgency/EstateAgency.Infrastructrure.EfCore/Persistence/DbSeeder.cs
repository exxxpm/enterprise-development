using EstateAgency.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructrure.EfCore.Persistence;

public static class DbSeeder
{
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
