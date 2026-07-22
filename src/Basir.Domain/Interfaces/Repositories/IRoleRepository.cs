using Basir.Domain.Entities.Identity;

namespace Basir.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<ApplicationRole?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ApplicationRole?> GetByNameAsync(string normalizedName, CancellationToken ct = default);
    Task<IReadOnlyList<ApplicationRole>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ApplicationRole role, CancellationToken ct = default);
    Task AssignToUserAsync(Guid userId, string roleName, CancellationToken ct = default);
}
