using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAuthApp.Auth;
using MvcAuthApp.Data;
using MvcAuthApp.Models;

namespace MvcAuthApp.Controllers;

/// <summary>Same posts as the HTML /Post pages. Policies match those actions.</summary>
[Authorize(Policy = Policies.CanReadPosts)]
[Tags("Posts")]
public class PostsApiController : ApiController
{
    private readonly IPostRepository _posts;
    private readonly IUnitOfWork _uow;
    private readonly IAuthorizationService _authz;

    public PostsApiController(IPostRepository posts, IUnitOfWork uow, IAuthorizationService authz)
    {
        _posts = posts;
        _uow = uow;
        _authz = authz;
    }

    /// <summary>List posts. Needs <c>posts.read</c>.</summary>
    /// <response code="200">Rows the HTML list also shows.</response>
    /// <response code="401">No proof.</response>
    /// <response code="403">Signed in but no posts.read (guest).</response>
    [HttpGet("posts")]
    [ProducesResponseType(typeof(IReadOnlyList<PostView>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<PostView>>> List(CancellationToken cancellationToken)
    {
        return Ok(PostView.Many(await _posts.ListAsync(cancellationToken)));
    }

    /// <summary>One post by id. Same <c>posts.read</c> policy as the list.</summary>
    /// <response code="200">Found.</response>
    /// <response code="404">No row with that id.</response>
    [HttpGet("posts/{id:int}")]
    [ProducesResponseType(typeof(PostView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostView>> Get(int id, CancellationToken cancellationToken)
    {
        Post? post = await _posts.GetAsync(id, cancellationToken);
        if (post == null)
        {
            return NotFound();
        }

        return Ok(PostView.From(post));
    }

    /// <summary>Create a post. Needs Editor or Admin plus <c>posts.write</c>. Author is the caller.</summary>
    /// <response code="201">Created. Location is GET /api/posts/{id}.</response>
    /// <response code="403">Reader or guest cannot write.</response>
    [HttpPost("posts")]
    [Authorize(Policy = Policies.CanWritePosts)]
    [ProducesResponseType(typeof(PostView), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PostView>> Create(PostWrite body, CancellationToken cancellationToken)
    {
        Post post = new Post
        {
            Title = body.Title,
            Content = body.Content
        };
        if (User.Identity != null && User.Identity.Name != null)
        {
            post.Author = User.Identity.Name;
        }

        _posts.Add(post);
        await _uow.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = post.Id }, PostView.From(post));
    }

    /// <summary>Update title and content. Resource check: author or Admin. Same as HTML Edit.</summary>
    /// <response code="200">Saved.</response>
    /// <response code="403">Signed in, not the author, not Admin.</response>
    /// <response code="404">No row.</response>
    [HttpPut("posts/{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(PostView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostView>> Update(int id, PostWrite body, CancellationToken cancellationToken)
    {
        Post? post = await _posts.FindAsync(id, cancellationToken);
        if (post == null)
        {
            return NotFound();
        }

        AuthorizationResult allowed = await _authz.AuthorizeAsync(User, post, Policies.EditPost);
        if (!allowed.Succeeded)
        {
            return Forbid();
        }

        post.Title = body.Title;
        post.Content = body.Content;
        await _uow.SaveChangesAsync(cancellationToken);
        return Ok(PostView.From(post));
    }
}
