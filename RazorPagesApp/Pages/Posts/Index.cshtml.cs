using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages.Posts;

public class IndexModel : PageModel
{
    private readonly PostStore _store;

    public IndexModel(PostStore store)
    {
        _store = store;
    }

    public IReadOnlyList<Post> Posts { get; set; } = new List<Post>();

    public void OnGet()
    {
        Posts = _store.All();
    }

    public IActionResult OnPostDelete(int id)
    {
        _store.Remove(id);
        return RedirectToPage();
    }
}

