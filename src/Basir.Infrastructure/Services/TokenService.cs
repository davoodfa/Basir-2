using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Basir.Application.Auth.DTOs;
using Basir.Application.Auth.Interfaces;
using Basir.Domain.Common;
using Basir.Domain.Interfaces.Repositories;
using Basir.Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace Basir.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwt;
    private readonly IUserRepository _users;

    public TokenService(JwtOptions jwt, IUserRepository users)
    {
        _jwt = jwt;
        _users = users;
    }

    public TokenDto GenerateToken(string userId, string userName, IReadOnlyList<string> roles)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(r => new Claim("role", r)));

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return new TokenDto(accessToken, refreshToken, expiresAt);
    }

    public async Task<Result<TokenDto>> RefreshAsync(Guid userId, string refreshToken, CancellationToken ct = default)
    {
        var stored = await _users.GetRefreshTokenAsync(userId, ct);
        if (stored is null || !CryptographicEquals(stored, refreshToken))
            return Result<TokenDto>.Failure("Invalid refresh token.");

        var expiry = await _users.GetRefreshTokenExpiryAsync(userId, ct);
        if (expiry.HasValue && expiry.Value <= DateTime.UtcNow)
        {
            await _users.RevokeRefreshTokenAsync(userId, ct);
            return Result<TokenDto>.Failure("Refresh token has expired. Please log in again.");
        }

        var user = await _users.GetByIdAsync(userId, ct);
        if (user is null || user.Status != Domain.Enums.UserStatus.Active)
            return Result<TokenDto>.Failure("User is not active.");

        var roles = await _users.GetRolesAsync(userId, ct);
        var tokens = GenerateToken(userId.ToString(), user.UserName ?? user.Email!, roles);

        var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays);
        await _users.AddRefreshTokenAsync(userId, tokens.RefreshToken, refreshTokenExpiry, ct);
        return Result<TokenDto>.Success(tokens);
    }

    public Task<string> GenerateEmailConfirmationTokenAsync(Guid userId, CancellationToken ct = default)
        => _users.GenerateEmailConfirmationTokenAsync(userId, ct);

    public Task<string> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default)
        => _users.GeneratePasswordResetTokenAsync(userId, ct);

    public Task<string> GenerateTwoFactorTokenAsync(Guid userId, CancellationToken ct = default)
        => _users.GetTwoFactorTokenAsync(userId, ct);

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private static bool CryptographicEquals(string a, string b)
    {
        var x = Encoding.UTF8.GetBytes(a);
        var y = Encoding.UTF8.GetBytes(b);
        return CryptographicOperations.FixedTimeEquals(x, y);
    }
}
