using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Posts;

public class CreateModel : PageModel
{
    private readonly PostStore _store;

    public CreateModel(PostStore store)
    {
        _store = store;
    }

    [BindProperty]
    public Post Post { get; set; } = new Post();

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        _store.Add(Post);
        return RedirectToPage("/Posts/Index");
    }
}
