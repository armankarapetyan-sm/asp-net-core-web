using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers;

public class AdminController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
