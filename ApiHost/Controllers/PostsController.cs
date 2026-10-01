using ApiHost.Data;
using ApiHost.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiHost.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly PostStore _store;

    public PostsController(PostStore store)
    {
        _store = store;
    }

    [HttpGet]
    public IActionResult List()
    {
        return Ok(_store.All());
    }

    [HttpPost]
    public IActionResult Create(Post post)
    {
        _store.Add(post);
        return Ok(post);
    }
}
