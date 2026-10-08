using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

/// <summary>Issues a JWT. This is the login for Swagger and other API clients, not for HTML pages.</summary>
[AllowAnonymous]
[Tags("Auth")]
public class TokenApiController : ApiController
{
    private readonly IUserRepository _users;
    private readonly TokenService _tokens;

    public TokenApiController(IUserRepository users, TokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    /// <summary>Exchange a mock user name and password for a Bearer token.</summary>
    /// <remarks>
    /// Same accounts as /Account/Login: admin/admin123, editor/editor123, reader/reader123, guest/guest123.
    /// Copy accessToken into Authorize → Bearer. A page click cannot send this header.
    /// </remarks>
    /// <response code="200">Token for that person.</response>
    /// <response code="401">Unknown user or wrong password.</response>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorBody), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> Create(Credentials request, CancellationToken cancellationToken)
    {
        AppUser? user = await _users.FindAsync(request.UserName, cancellationToken);
        if (user == null || !_users.Verify(user, request.Password))
        {
            return Unauthorized(new ErrorBody { Error = "Wrong user name or password." });
        }

        return Ok(_tokens.Create(user));
    }
}
