using Basir.Domain.Entities.Globalization;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Globalization.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly AppIdentityDbContext _context;

    public LanguageRepository(AppIdentityDbContext context) => _context = context;

    public Task<List<Language>> GetAllAsync(CancellationToken ct = default)
        => _context.Set<Language>().OrderBy(l => l.SortOrder).ToListAsync(ct);

    public Task<List<Language>> GetEnabledAsync(CancellationToken ct = default)
        => _context.Set<Language>().Where(l => l.IsEnabled).OrderBy(l => l.SortOrder).ToListAsync(ct);

    public Task<Language?> GetByIdAsync(int id, CancellationToken ct = default)
        => _context.Set<Language>().FirstOrDefaultAsync(l => l.Id == id, ct);

    public Task<Language?> GetByCodeAsync(string code, CancellationToken ct = default)
        => _context.Set<Language>().FirstOrDefaultAsync(l => l.Code == code, ct);

    public Task<Language?> GetDefaultAsync(CancellationToken ct = default)
        => _context.Set<Language>().FirstOrDefaultAsync(l => l.IsDefault, ct);
}
