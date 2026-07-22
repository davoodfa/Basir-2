using Basir.Domain.Entities.Globalization;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Globalization.Repositories;

public class ThemeRepository : IThemeRepository
{
    private readonly AppIdentityDbContext _context;

    public ThemeRepository(AppIdentityDbContext context) => _context = context;

    public Task<List<Theme>> GetAllAsync(CancellationToken ct = default)
        => _context.Set<Theme>().OrderBy(t => t.SortOrder).ToListAsync(ct);

    public Task<List<Theme>> GetEnabledAsync(CancellationToken ct = default)
        => _context.Set<Theme>().Where(t => t.IsEnabled).OrderBy(t => t.SortOrder).ToListAsync(ct);

    public Task<Theme?> GetByIdAsync(int id, CancellationToken ct = default)
        => _context.Set<Theme>().FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<Theme?> GetByNameAsync(string name, CancellationToken ct = default)
        => _context.Set<Theme>().FirstOrDefaultAsync(t => t.Name == name, ct);

    public Task<Theme?> GetDefaultAsync(CancellationToken ct = default)
        => _context.Set<Theme>().FirstOrDefaultAsync(t => t.IsDefault, ct);
}
