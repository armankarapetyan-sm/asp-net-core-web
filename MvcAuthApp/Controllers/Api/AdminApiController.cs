using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

/// <summary>Same checks as the HTML Admin pages.</summary>
[Tags("Admin")]
public class AdminApiController : ApiController
{
    private readonly IUserRepository _users;

    public AdminApiController(IUserRepository users)
    {
        _users = users;
    }

    /// <summary>List users. Needs Admin plus <c>users.manage</c>.</summary>
    [HttpGet("users")]
    [Authorize(Policy = Policies.CanManageUsers)]
    [ProducesResponseType(typeof(IReadOnlyList<UserPublic>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<UserPublic>>> Users(CancellationToken cancellationToken)
    {
        return Ok(UserPublic.Many(await _users.ListAsync(cancellationToken)));
    }

    /// <summary>Succeeds only for Admin. No scope policy — compare with GET /api/users.</summary>
    [HttpGet("admin/roles-only")]
    [Authorize(Roles = AppRoles.Admin)]
    [ProducesResponseType(typeof(RolesCheckResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<RolesCheckResponse> RolesOnly()
    {
        RolesCheckResponse body = new RolesCheckResponse { Ok = true };
        if (User.Identity != null && User.Identity.Name != null)
        {
            body.Name = User.Identity.Name;
        }

        return Ok(body);
    }
}
