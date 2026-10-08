using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

[AllowAnonymous]
public class XssController : Controller
{
    private readonly IWallRepository _wall;
    private readonly IUnitOfWork _uow;
    private readonly OpenNoteCookieSettings _openNote;
    private readonly XssLabStore _lab;

    public XssController(
        IWallRepository wall,
        IUnitOfWork uow,
        IOptions<CookieSettings> cookies,
        XssLabStore lab)
    {
        _wall = wall;
        _uow = uow;
        _openNote = cookies.Value.OpenNote;
        _lab = lab;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        SetOpenNoteCookie();
        return View(new XssPage
        {
            Notes = WallNoteView.Many(await _wall.ListAsync(cancellationToken))
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post(WallNoteForm form, CancellationToken cancellationToken)
    {
        AddNote(form.Body, false);
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PostSafe(WallNoteForm form, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(form.Body))
        {
            return RedirectToAction("Index");
        }

        string encoded = HtmlEncoder.Default.Encode(form.Body.Trim());
        AddNote(encoded, true);
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Demo(CancellationToken cancellationToken)
    {
        AddNote(DemoPayload, false);
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DemoSafe(CancellationToken cancellationToken)
    {
        AddNote(HtmlEncoder.Default.Encode(DemoPayload), true);
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Clear(CancellationToken cancellationToken)
    {
        _wall.Clear();
        _wall.Add(new WallNote
        {
            Author = "admin",
            Body = "Hello. This line is plain text."
        });
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Lab(bool cleared)
    {
        PlantLabBait();
        return View(new XssLabPage
        {
            Comments = _lab.List(),
            Attacks = XssLabAttacks.All,
            Cleared = cleared
        });
    }

    [HttpPost]
    public IActionResult Attack(string kind)
    {
        XssLabAttack? attack = XssLabAttacks.Find(kind);
        if (attack != null)
        {
            _lab.Add("attacker", attack.Html);
        }

        return RedirectToAction("Lab");
    }

    [HttpPost]
    public IActionResult Reset()
    {
        _lab.Clear();
        Response.Cookies.Delete(CookieNames.SavedLogin);
        return RedirectToAction("Lab", new { cleared = true });
    }

    private const string DemoPayload =
        "Nice post. <img src=\"/missing-xss\" alt=\"\" onerror=\"window.showStolen && window.showStolen()\">";

    private void AddNote(string? body, bool protectedSave)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return;
        }

        string author = "anonymous";
        if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.Name != null)
        {
            author = User.Identity.Name;
        }

        _wall.Add(new WallNote
        {
            Author = author,
            Body = body.Trim(),
            ProtectedSave = protectedSave
        });
    }

    private void SetOpenNoteCookie()
    {
        Response.Cookies.Append(
            _openNote.Name,
            "classroom-secret",
            _openNote.ToCookieOptions());
    }

    private void PlantLabBait()
    {
        SetOpenNoteCookie();
        Response.Cookies.Append(
            CookieNames.SavedLogin,
            "editor:editor123",
            _openNote.ToCookieOptions());
    }
}
