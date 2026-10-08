using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

public class AdminController : Controller
{
    private readonly IUserRepository _users;

    public AdminController(IUserRepository users)
    {
        _users = users;
    }

    [Authorize(Policy = Policies.CanManageUsers)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(UserPublic.Many(await _users.ListAsync(cancellationToken)));
    }

    [Authorize(Roles = AppRoles.Admin)]
    public IActionResult RolesOnly()
    {
        return View();
    }
}
