using Basir.Domain.Entities.Identity;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Basir.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private static class TokenNames
    {
        public const string Provider = "Basir";
        public const string RefreshToken = "RefreshToken";
        public const string RefreshTokenExpiry = "RefreshTokenExpiry";
    }

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppIdentityDbContext _db;

    public UserRepository(UserManager<ApplicationUser> userManager, AppIdentityDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public async Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _userManager.FindByIdAsync(id.ToString());

    public async Task<ApplicationUser?> GetByEmailAsync(string normalizedEmail, CancellationToken ct = default)
        => await _userManager.FindByEmailAsync(normalizedEmail);

    public async Task<ApplicationUser?> GetByUserNameAsync(string normalizedUserName, CancellationToken ct = default)
        => await _userManager.FindByNameAsync(normalizedUserName);

    public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken ct = default)
    {
        return await _db.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Join(_db.Roles.AsNoTracking(),
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name!)
            .ToListAsync(ct);
    }

    public async Task<bool> IsEmailTakenAsync(string normalizedEmail, Guid? exceptUserId = null, CancellationToken ct = default)
    {
        var query = _db.Users.AsNoTracking().Where(u => u.NormalizedEmail == normalizedEmail);
        if (exceptUserId.HasValue)
            query = query.Where(u => u.Id != exceptUserId.Value);
        return await query.AnyAsync(ct);
    }

    public async Task<IReadOnlyList<ApplicationUser>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await _db.Users
            .OrderBy(u => u.UserName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<bool> CreateWithPasswordAsync(ApplicationUser user, string password, CancellationToken ct = default)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return string.Empty;
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<bool> ConfirmEmailAsync(Guid userId, string token, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return string.Empty;
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<bool> ResetPasswordAsync(Guid userId, string token, string newPassword, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> SetTwoFactorEnabledAsync(Guid userId, bool enabled, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var result = await _userManager.SetTwoFactorEnabledAsync(user, enabled);
        return result.Succeeded;
    }

    public async Task<string> GetTwoFactorTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return string.Empty;
        return await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(Guid userId, string code, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        return await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code);
    }

    public async Task<bool> AddRefreshTokenAsync(Guid userId, string token, DateTime expiresAt, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        await _userManager.SetAuthenticationTokenAsync(user, TokenNames.Provider, TokenNames.RefreshToken, token);
        await _userManager.SetAuthenticationTokenAsync(user, TokenNames.Provider, TokenNames.RefreshTokenExpiry, expiresAt.ToString("O"));
        return true;
    }

    public async Task<string?> GetRefreshTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;
        return await _userManager.GetAuthenticationTokenAsync(user, TokenNames.Provider, TokenNames.RefreshToken);
    }

    public async Task<DateTime?> GetRefreshTokenExpiryAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;
        var value = await _userManager.GetAuthenticationTokenAsync(user, TokenNames.Provider, TokenNames.RefreshTokenExpiry);
        if (string.IsNullOrEmpty(value)) return null;
        if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiry))
            return expiry;
        return null;
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return;
        await _userManager.RemoveAuthenticationTokenAsync(user, TokenNames.Provider, TokenNames.RefreshToken);
        await _userManager.RemoveAuthenticationTokenAsync(user, TokenNames.Provider, TokenNames.RefreshTokenExpiry);
    }
}
