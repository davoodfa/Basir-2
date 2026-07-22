using Basir.Domain.Entities.Globalization;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Globalization.Repositories;

public class UserPreferenceRepository : IUserPreferenceRepository
{
    private readonly AppIdentityDbContext _context;

    public UserPreferenceRepository(AppIdentityDbContext context) => _context = context;

    public Task<UserPreference?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => _context.Set<UserPreference>().FirstOrDefaultAsync(up => up.UserId == userId, ct);

    public async Task AddAsync(UserPreference preference, CancellationToken ct = default)
    {
        await _context.Set<UserPreference>().AddAsync(preference, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(UserPreference preference, CancellationToken ct = default)
    {
        _context.Set<UserPreference>().Update(preference);
        await _context.SaveChangesAsync(ct);
    }
}
