using MvcAuthApp.Models;

namespace MvcAuthApp.Data;

public interface IWallRepository
{
    Task<IReadOnlyList<WallNote>> ListAsync(CancellationToken cancellationToken);
    void Add(WallNote note);
    void Clear();
}
