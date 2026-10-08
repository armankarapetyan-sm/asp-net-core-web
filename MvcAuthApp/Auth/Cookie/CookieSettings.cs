using Microsoft.AspNetCore.Authentication.Cookies;

namespace MvcAuthApp.Auth;

public class CookieSettings
{
    public const string SectionName = "Cookies";

    public AuthCookieSettings Auth { get; set; } = new AuthCookieSettings();
    public OpenNoteCookieSettings OpenNote { get; set; } = new OpenNoteCookieSettings();

    public void FillGaps()
    {
        if (Auth == null)
        {
            Auth = new AuthCookieSettings();
        }

        if (OpenNote == null)
        {
            OpenNote = new OpenNoteCookieSettings();
        }
    }
}

public class AuthCookieSettings
{
    public string Name { get; set; } = CookieNames.Auth;
    public string LoginPath { get; set; } = "/Account/Login";
    public string AccessDeniedPath { get; set; } = "/Account/Denied";
    public int ExpireHours { get; set; } = 8;
    public bool SlidingExpiration { get; set; } = true;
    public bool HttpOnly { get; set; } = true;
    public string SameSite { get; set; } = "Lax";
    public string SecurePolicy { get; set; } = "SameAsRequest";
    public int RememberMeDays { get; set; } = 30;

    public void Apply(CookieAuthenticationOptions options)
    {
        options.LoginPath = LoginPath;
        options.AccessDeniedPath = AccessDeniedPath;
        options.ExpireTimeSpan = TimeSpan.FromHours(ExpireHours);
        options.SlidingExpiration = SlidingExpiration;
        options.Cookie.Name = Name;
        options.Cookie.HttpOnly = HttpOnly;
        options.Cookie.SameSite = ParseSameSite(SameSite);
        options.Cookie.SecurePolicy = ParseSecurePolicy(SecurePolicy);
    }

    private static SameSiteMode ParseSameSite(string value)
    {
        SameSiteMode mode;
        if (Enum.TryParse(value, true, out mode))
        {
            return mode;
        }

        return SameSiteMode.Lax;
    }

    private static CookieSecurePolicy ParseSecurePolicy(string value)
    {
        CookieSecurePolicy policy;
        if (Enum.TryParse(value, true, out policy))
        {
            return policy;
        }

        return CookieSecurePolicy.SameAsRequest;
    }
}

public class OpenNoteCookieSettings
{
    public string Name { get; set; } = CookieNames.OpenNote;
    public int Hours { get; set; } = 1;
    public bool HttpOnly { get; set; }
    public string SameSite { get; set; } = "Lax";

    public CookieOptions ToCookieOptions()
    {
        SameSiteMode mode;
        if (!Enum.TryParse(SameSite, true, out mode))
        {
            mode = SameSiteMode.Lax;
        }

        return new CookieOptions
        {
            Path = "/",
            MaxAge = TimeSpan.FromHours(Hours),
            HttpOnly = HttpOnly,
            SameSite = mode
        };
    }
}
