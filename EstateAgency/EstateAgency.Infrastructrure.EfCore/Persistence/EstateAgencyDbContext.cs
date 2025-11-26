using EstateAgency.Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructrure.EfCore.Persistence;

/// <summary>
/// EF Core database context for the Estate Agency application, managing counterparties, properties, and applications.
/// </summary>
/// <param name="options">Database context options.</param>
public class EstateAgencyDbContext(DbContextOptions<EstateAgencyDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Counterparties table.
    /// </summary>
    public DbSet<Counterparty> Counterparties { get; set; }

    /// <summary>
    /// Properties table.
    /// </summary>
    public DbSet<Property> Properties { get; set; }

    /// <summary>
    /// Applications table.
    /// </summary>
    public DbSet<Application> Applications { get; set; }

    /// <summary>
    /// Configures entity mappings, relationships, conversions, and precision for decimal fields.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Application>(entity =>
        {
            entity.Property(a => a.Type).HasConversion<string>();
            entity.HasOne(a => a.Counterparty)
                  .WithMany(c => c.Applications)
                  .HasForeignKey(a => a.CounterpartyId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(a => a.Property)
                  .WithMany(p => p.Applications)
                  .HasForeignKey(a => a.PropertyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.Property(p => p.Type).HasConversion<string>();
            entity.Property(p => p.Purpose).HasConversion<string>();
            entity.Property(p => p.TotalArea).HasPrecision(10, 2);
            entity.Property(p => p.CeilingHeight).HasPrecision(5, 2);
        });
    }
}
