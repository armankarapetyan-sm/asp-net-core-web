using ApiHost.Data;
using Microsoft.AspNetCore.Mvc;

namespace ApiHost.Controllers;

public class HomeController : Controller
{
    private readonly PostStore _store;

    public HomeController(PostStore store)
    {
        _store = store;
    }

    public IActionResult Index()
    {
        return View(_store.All());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Status()
    {
        ViewData["Count"] = _store.All().Count;
        return View();
    }
}
