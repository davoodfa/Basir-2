using Basir.Domain.Entities.Identity;

namespace Basir.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ApplicationUser?> GetByEmailAsync(string normalizedEmail, CancellationToken ct = default);
    Task<ApplicationUser?> GetByUserNameAsync(string normalizedUserName, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken ct = default);
    Task<bool> IsEmailTakenAsync(string normalizedEmail, Guid? exceptUserId = null, CancellationToken ct = default);
    Task<IReadOnlyList<ApplicationUser>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);

    Task<bool> CreateWithPasswordAsync(ApplicationUser user, string password, CancellationToken ct = default);
    Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken ct = default);
    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ConfirmEmailAsync(Guid userId, string token, CancellationToken ct = default);
    Task<string> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(Guid userId, string token, string newPassword, CancellationToken ct = default);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken ct = default);
    Task<bool> SetTwoFactorEnabledAsync(Guid userId, bool enabled, CancellationToken ct = default);
    Task<string> GetTwoFactorTokenAsync(Guid userId, CancellationToken ct = default);
    Task<bool> VerifyTwoFactorTokenAsync(Guid userId, string code, CancellationToken ct = default);
    Task<bool> AddRefreshTokenAsync(Guid userId, string token, DateTime expiresAt, CancellationToken ct = default);
    Task<string?> GetRefreshTokenAsync(Guid userId, CancellationToken ct = default);
    Task<DateTime?> GetRefreshTokenExpiryAsync(Guid userId, CancellationToken ct = default);
    Task RevokeRefreshTokenAsync(Guid userId, CancellationToken ct = default);
}
