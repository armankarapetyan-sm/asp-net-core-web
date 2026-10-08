using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuthApp.Auth;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

/// <summary>Who the current proof is. Cookie or Bearer — same shape.</summary>
[Authorize]
[Tags("Auth")]
public class MeApiController : ApiController
{
    /// <summary>Return the name, role, scopes, and which scheme authenticated you.</summary>
    /// <response code="200">Signed in.</response>
    /// <response code="401">No cookie or token.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(MeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<MeResponse> Get()
    {
        MeResponse me = new MeResponse();
        if (User.Identity != null && User.Identity.Name != null)
        {
            me.Name = User.Identity.Name;
            me.Proof = AppClaims.ProofName(User.Identity.AuthenticationType);
        }

        Claim? role = User.FindFirst(ClaimTypes.Role);
        if (role != null)
        {
            me.Role = role.Value;
        }

        foreach (Claim claim in User.FindAll(AppScopes.ClaimType))
        {
            me.Scopes.Add(claim.Value);
        }

        return Ok(me);
    }
}
