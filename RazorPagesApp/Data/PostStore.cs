using RazorPagesApp.Models;

namespace RazorPagesApp.Data;

public class PostStore
{
    private readonly List<Post> _posts = new List<Post>();
    private int _nextId = 1;

    public PostStore()
    {
        Add(new Post { Title = "Hello Razor Pages", Content = "This page has no controller." });
    }

    public IReadOnlyList<Post> All()
    {
        return _posts;
    }

    public void Add(Post post)
    {
        post.Id = _nextId;
        _nextId++;
        _posts.Add(post);
    }

    public void Remove(int id)
    {
        Post? found = null;
        foreach (Post post in _posts)
        {
            if (post.Id == id)
            {
                found = post;
                break;
            }
        }

        if (found != null)
        {
            _posts.Remove(found);
        }
    }
}
