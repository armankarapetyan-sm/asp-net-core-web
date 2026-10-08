using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MvcAuthApp.Models;

namespace MvcAuthApp.Auth;

public static class AppClaims
{
    public static List<Claim> For(string name, string role, IEnumerable<string> scopes)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role)
        };
        foreach (string scope in scopes)
        {
            claims.Add(new Claim(AppScopes.ClaimType, scope));
        }

        return claims;
    }

    public static List<Claim> For(AppUser user)
    {
        return For(user.UserName, user.Role, user.Scopes);
    }

    public static string ProofName(string? scheme)
    {
        if (scheme == JwtBearerDefaults.AuthenticationScheme
            || scheme == "AuthenticationTypes.Federation")
        {
            return "Bearer";
        }

        return scheme ?? "";
    }
}
