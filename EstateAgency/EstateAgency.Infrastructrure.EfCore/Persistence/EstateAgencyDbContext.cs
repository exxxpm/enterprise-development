using EstateAgency.Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructrure.EfCore.Persistence;

public class EstateAgencyDbContext(DbContextOptions<EstateAgencyDbContext> options) : DbContext(options)
{
    public DbSet<Counterparty> Counterparties => Set<Counterparty>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Application>()
            .Property(a => a.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.Purpose)
            .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .Property(p => p.TotalArea)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Property>()
            .Property(p => p.CeilingHeight)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Counterparty)
            .WithMany(c => c.Applications)
            .HasForeignKey(a => a.CounterpartyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Property)
            .WithMany(p => p.Applications)
            .HasForeignKey(a => a.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
