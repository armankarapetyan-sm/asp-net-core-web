using MvcAuthApp.Models;

namespace MvcAuthApp.Data;

public interface IPostRepository
{
    Task<IReadOnlyList<Post>> ListAsync(CancellationToken cancellationToken);
    Task<Post?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Post?> FindAsync(int id, CancellationToken cancellationToken);
    void Add(Post post);
}
