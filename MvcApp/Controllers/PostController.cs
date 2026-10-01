using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers;

public class PostController : Controller
{
    private static List<Post> _posts = new List<Post>
    {
        new Post { Title = "Hello MVC", Content = "This list lives in memory on the server." }
    };

    [HttpGet]
    public IActionResult Index()
    {
        return View(_posts);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Post post)
    {
        _posts.Add(post);
        TempData["Ok"] = "Post saved.";
        return RedirectToAction("Index");
    }
}
