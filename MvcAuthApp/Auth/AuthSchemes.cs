using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MvcAuthApp.Auth;

public static class AuthSchemes
{
    public const string CookieOrJwt = "CookieOrJwt";

    public static string Select(HttpContext context)
    {
        string? authorization = context.Request.Headers.Authorization;
        if (!string.IsNullOrEmpty(authorization)
            && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return JwtBearerDefaults.AuthenticationScheme;
        }

        return CookieAuthenticationDefaults.AuthenticationScheme;
    }
}
