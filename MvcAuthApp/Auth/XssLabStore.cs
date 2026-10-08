using MvcAuthApp.Models;

namespace MvcAuthApp.Auth;

public class XssLabStore
{
    private readonly List<WallNoteView> _comments = new List<WallNoteView>();
    private readonly object _gate = new object();

    public IReadOnlyList<WallNoteView> List()
    {
        lock (_gate)
        {
            return new List<WallNoteView>(_comments);
        }
    }

    public void Add(string author, string body)
    {
        lock (_gate)
        {
            _comments.Add(new WallNoteView
            {
                Author = author,
                Body = body
            });
        }
    }

    public void Clear()
    {
        lock (_gate)
        {
            _comments.Clear();
        }
    }
}
