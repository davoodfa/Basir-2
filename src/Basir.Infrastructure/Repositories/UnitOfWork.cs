using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppIdentityDbContext _db;

    public UnitOfWork(AppIdentityDbContext db) => _db = db;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}
