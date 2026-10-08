using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

public class PostController : Controller
{
    private readonly IPostRepository _posts;
    private readonly IUnitOfWork _uow;
    private readonly IAuthorizationService _authz;

    public PostController(IPostRepository posts, IUnitOfWork uow, IAuthorizationService authz)
    {
        _posts = posts;
        _uow = uow;
        _authz = authz;
    }

    [Authorize(Policy = Policies.CanReadPosts)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(PostView.Many(await _posts.ListAsync(cancellationToken)));
    }

    [HttpGet]
    [Authorize(Policy = Policies.CanWritePosts)]
    public IActionResult Create()
    {
        return View(new PostForm());
    }

    [HttpPost]
    [Authorize(Policy = Policies.CanWritePosts)]
    public async Task<IActionResult> Create(PostForm form, CancellationToken cancellationToken)
    {
        Post post = new Post
        {
            Title = form.Title,
            Content = form.Content
        };
        if (User.Identity != null && User.Identity.Name != null)
        {
            post.Author = User.Identity.Name;
        }

        _posts.Add(post);
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        Post? post = await _posts.GetAsync(id, cancellationToken);
        if (post == null)
        {
            return NotFound();
        }

        AuthorizationResult allowed = await _authz.AuthorizeAsync(User, post, Policies.EditPost);
        if (!allowed.Succeeded)
        {
            return Forbid();
        }

        return View(new PostForm
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(PostForm form, CancellationToken cancellationToken)
    {
        Post? post = await _posts.FindAsync(form.Id, cancellationToken);
        if (post == null)
        {
            return NotFound();
        }

        AuthorizationResult allowed = await _authz.AuthorizeAsync(User, post, Policies.EditPost);
        if (!allowed.Succeeded)
        {
            return Forbid();
        }

        post.Title = form.Title;
        post.Content = form.Content;
        await _uow.SaveChangesAsync(cancellationToken);
        return RedirectToAction("Index");
    }
}
