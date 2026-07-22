using Basir.Domain.Entities.Identity;
using Basir.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<ApplicationRole?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _roleManager.FindByIdAsync(id.ToString());

    public async Task<ApplicationRole?> GetByNameAsync(string normalizedName, CancellationToken ct = default)
        => await _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName);

    public async Task<IReadOnlyList<ApplicationRole>> GetAllAsync(CancellationToken ct = default)
        => await _roleManager.Roles.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(ApplicationRole role, CancellationToken ct = default)
        => await _roleManager.CreateAsync(role);

    public async Task AssignToUserAsync(Guid userId, string roleName, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is not null && !await _userManager.IsInRoleAsync(user, roleName))
            await _userManager.AddToRoleAsync(user, roleName);
    }
}
