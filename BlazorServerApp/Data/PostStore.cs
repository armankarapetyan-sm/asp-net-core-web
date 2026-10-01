using BlazorServerApp.Models;

namespace BlazorServerApp.Data;

public class PostStore
{
    private readonly List<Post> _posts = new List<Post>
    {
        new Post { Title = "Hello Blazor Server", Content = "Clicks stay on the server over SignalR." }
    };

    public IReadOnlyList<Post> All()
    {
        return _posts;
    }

    public void Add(Post post)
    {
        _posts.Add(post);
    }
}
