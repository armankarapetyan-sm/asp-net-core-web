using ApiHost.Models;

namespace ApiHost.Data;

public class PostStore
{
    private readonly List<Post> _posts = new List<Post>();

    public PostStore()
    {
        _posts.Add(new Post { Title = "Hello", Content = "Seed from the API process" });
    }

    public IReadOnlyList<Post> All()
    {
        return _posts;
    }

    public void Add(Post post)
    {
        _posts.Add(post);
    }
}
