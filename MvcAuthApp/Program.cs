using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;

namespace MvcAuthApp;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddData(builder.Configuration);
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddSingleton<XssLabStore>();
        builder.Services.AddSingleton<IAuthorizationHandler, EditPostHandler>();
        builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection(CookieSettings.SectionName));
        builder.Services.PostConfigure<CookieSettings>(settings => settings.FillGaps());
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        CookieSettings cookies = builder.Configuration.GetSection(CookieSettings.SectionName).Get<CookieSettings>()
            ?? new CookieSettings();
        cookies.FillGaps();
        JwtOptions jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthSchemes.CookieOrJwt;
                options.DefaultChallengeScheme = AuthSchemes.CookieOrJwt;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                cookies.Auth.Apply(options);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = CookieApiRedirect.ToLogin,
                    OnRedirectToAccessDenied = CookieApiRedirect.ToDenied
                };
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ClockSkew = TimeSpan.FromMinutes(1),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            })
            .AddPolicyScheme(AuthSchemes.CookieOrJwt, AuthSchemes.CookieOrJwt, options =>
            {
                options.ForwardDefaultSelector = AuthSchemes.Select;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auth demo API",
                Description =
                    "Pages: cookie. API: Bearer from POST /api/token."
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "POST /api/token, then paste accessToken here (Swagger adds Bearer)."
            });
            options.DocInclusionPredicate((string _, ApiDescription desc) =>
            {
                return desc.RelativePath != null
                    && desc.RelativePath.StartsWith("api/", StringComparison.OrdinalIgnoreCase);
            });
            options.OperationFilter<SwaggerAuthFilter>();
            string xml = Path.Combine(AppContext.BaseDirectory, "MvcAuthApp.xml");
            if (File.Exists(xml))
            {
                options.IncludeXmlComments(xml, true);
            }
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanReadPosts, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(AppScopes.ClaimType, AppScopes.PostsRead);
            });
            options.AddPolicy(Policies.CanWritePosts, policy =>
            {
                policy.RequireRole(AppRoles.Editor, AppRoles.Admin);
                policy.RequireClaim(AppScopes.ClaimType, AppScopes.PostsWrite);
            });
            options.AddPolicy(Policies.CanManageUsers, policy =>
            {
                policy.RequireRole(AppRoles.Admin);
                policy.RequireClaim(AppScopes.ClaimType, AppScopes.UsersManage);
            });
            options.AddPolicy(Policies.EditPost, policy =>
            {
                policy.AddRequirements(new EditPostRequirement());
            });
        });

        WebApplication app = builder.Build();
        app.ApplyDatabase();
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = "swagger";
        });
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
}
