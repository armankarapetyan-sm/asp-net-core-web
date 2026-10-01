using BlazorWasmApp.Models;

namespace BlazorWasmApp.Data;

public class PostStore
{
    private readonly List<Post> _posts = new List<Post>
    {
        new Post { Title = "Hello WASM", Content = "This list lives in the browser tab." }
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
