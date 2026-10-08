namespace MvcAuthApp.Models;

public class PostView
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string Author { get; set; } = "";

    public static PostView From(Post post)
    {
        return new PostView
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author
        };
    }

    public static List<PostView> Many(IEnumerable<Post> posts)
    {
        List<PostView> rows = new List<PostView>();
        foreach (Post post in posts)
        {
            rows.Add(From(post));
        }

        return rows;
    }
}
