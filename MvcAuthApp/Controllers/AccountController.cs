using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _uow;
    private readonly TokenService _tokens;
    private readonly AuthCookieSettings _cookie;

    public AccountController(
        IUserRepository users,
        IUnitOfWork uow,
        TokenService tokens,
        IOptions<CookieSettings> cookies)
    {
        _users = users;
        _uow = uow;
        _tokens = tokens;
        _cookie = cookies.Value.Auth;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginForm());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginForm form, string? returnUrl, CancellationToken cancellationToken)
    {
        AppUser? user = await _users.FindAsync(form.UserName, cancellationToken);
        if (user == null || !_users.Verify(user, form.Password))
        {
            ViewData["Error"] = "Wrong user name or password.";
            ViewData["ReturnUrl"] = returnUrl;
            return View(form);
        }

        await SignInUser(user, form.RememberMe);
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterForm());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterForm form, CancellationToken cancellationToken)
    {
        UserWriteResult created = await _users.TryAddReaderAsync(form.UserName, form.Password, cancellationToken);
        if (created.User == null)
        {
            ViewData["Error"] = created.Error;
            return View(form);
        }

        await _uow.SaveChangesAsync(cancellationToken);
        await SignInUser(created.User, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Denied()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Token()
    {
        return View(new TokenForm());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Token(TokenForm form, CancellationToken cancellationToken)
    {
        AppUser? user = await _users.FindAsync(form.UserName, cancellationToken);
        if (user == null || !_users.Verify(user, form.Password))
        {
            ViewData["Error"] = "Wrong user name or password.";
            return View(form);
        }

        ViewData["Issued"] = _tokens.Create(user);
        return View(form);
    }

    private async Task SignInUser(AppUser user, bool rememberMe)
    {
        ClaimsIdentity identity = new ClaimsIdentity(
            AppClaims.For(user),
            CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties props = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(_cookie.RememberMeDays) : null
        };
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            props);
    }
}
