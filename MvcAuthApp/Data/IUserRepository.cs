using MvcAuthApp.Models;

namespace MvcAuthApp.Data;

public interface IUserRepository
{
    Task<IReadOnlyList<AppUser>> ListAsync(CancellationToken cancellationToken);
    Task<AppUser?> FindAsync(string userName, CancellationToken cancellationToken);
    bool Verify(AppUser user, string password);
    Task<UserWriteResult> TryAddReaderAsync(string userName, string password, CancellationToken cancellationToken);
}
