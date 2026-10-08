using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MvcAuthApp.Models;

namespace MvcAuthApp.Auth;

public class TokenService
{
    private readonly JwtOptions _jwt;

    public TokenService(IOptions<JwtOptions> jwt)
    {
        _jwt = jwt.Value;
    }

    public TokenResponse Create(AppUser user)
    {
        DateTimeOffset expires = DateTimeOffset.UtcNow.AddMinutes(_jwt.LifetimeMinutes);
        SigningCredentials creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
            SecurityAlgorithms.HmacSha256);
        JwtSecurityToken jwt = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            AppClaims.For(user),
            DateTime.UtcNow,
            expires.UtcDateTime,
            creds);

        return new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            TokenType = "Bearer",
            ExpiresUtc = expires,
            Name = user.UserName,
            Role = user.Role
        };
    }
}
