using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MvcAuthApp.Auth;

public static class CookieApiRedirect
{
    public static Task ToLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        if (IsApi(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    }

    public static Task ToDenied(RedirectContext<CookieAuthenticationOptions> context)
    {
        if (IsApi(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    }

    private static bool IsApi(HttpRequest request)
    {
        return request.Path.StartsWithSegments("/api");
    }
}
