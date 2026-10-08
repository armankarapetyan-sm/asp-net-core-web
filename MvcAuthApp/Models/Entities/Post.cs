namespace MvcAuthApp.Models;

/// <summary>A post. Same rows as the HTML pages.</summary>
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string Author { get; set; } = "";
}
