using EstateAgency.Domain.Entitites;
using EstateAgency.Infrastructrure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EstateAgency.Infrastructrure.EfCore.Repositories;

/// <summary>
/// Repository implementation for CRUD operations on Application entities with additional includings using EF Core.
/// </summary>
/// <param name="context">Database context options.</param>
public class ApplicationRepository(EstateAgencyDbContext context) : Repository<Application>(context)
{
    /// <summary>
    /// Database context for accessing the Estate Agency database.
    /// </summary>
    private readonly EstateAgencyDbContext _context = context;

    /// <summary>
    /// Retrieves all Application entities /> from the database.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    public override async Task<IEnumerable<Application>> GetAllAsync()
    {
        return await _context.Set<Application>()
            .Include(a => a.Counterparty)
            .Include(a => a.Property)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves an Application entity by its unique identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public override async Task<Application?> GetByIdAsync(int id)
    {
        return await _context.Set<Application>()
            .Include(a => a.Counterparty)
            .Include(a => a.Property)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
