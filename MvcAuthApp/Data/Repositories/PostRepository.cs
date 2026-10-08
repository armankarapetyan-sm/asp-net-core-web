using Microsoft.EntityFrameworkCore;
using MvcAuthApp.Models;

namespace MvcAuthApp.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _db;

    public PostRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Post>> ListAsync(CancellationToken cancellationToken)
    {
        return await _db.Posts
            .AsNoTracking()
            .OrderBy(post => post.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<Post?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return _db.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(post => post.Id == id, cancellationToken);
    }

    public Task<Post?> FindAsync(int id, CancellationToken cancellationToken)
    {
        return _db.Posts.FirstOrDefaultAsync(post => post.Id == id, cancellationToken);
    }

    public void Add(Post post)
    {
        _db.Posts.Add(post);
    }
}
