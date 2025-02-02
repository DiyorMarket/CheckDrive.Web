using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace CheckDrive.Web.Helpers;

internal static class JwtHelper
{
    public static bool IsValid([NotNullWhen(true)] string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var jwtToken = ValidateOrThrow(token);

        var expirationDate = jwtToken.ValidTo;

        return expirationDate > DateTime.UtcNow;
    }

    public static string GetAccountId(string token)
    {
        var jwtToken = ValidateOrThrow(token);

        return jwtToken.Claims.First(c => c.Type == ClaimTypes.PrimarySid).Value;
    }

    public static string GetUserId(string token)
    {
        var jwtToken = ValidateOrThrow(token);

        return jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }

    public static string GetUserRole(string token)
    {
        var jwtToken = ValidateOrThrow(token);

        return jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;
    }

    private static JwtSecurityToken ValidateOrThrow(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException(nameof(token));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.CanReadToken(token))
        {
            throw new SecurityTokenValidationException("Invalid JWT token.");
        }

        if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
        {
            throw new SecurityTokenException("Could not read JWT Security token.");
        }

        return jwtToken;
    }
}
