using System.Text;
using Basir.Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace Basir.Infrastructure.Auth;

public static class JwtTokenValidator
{
    public static TokenValidationParameters CreateValidationParameters(JwtOptions jwt)
    {
        if (string.IsNullOrEmpty(jwt.Key) || jwt.Key.Length < 32)
            throw new InvalidOperationException(
                "JWT signing key must be at least 32 characters (256 bits) for HMAC-SHA256.");

        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            AuthenticationType = "Jwt"
        };
    }
}
