using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcAuthApp.Auth;
using MvcAuthApp.Models;

namespace MvcAuthApp.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<AppUser> _hasher = new PasswordHasher<AppUser>();

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<AppUser>> ListAsync(CancellationToken cancellationToken)
    {
        return await _db.Users
            .AsNoTracking()
            .OrderBy(user => user.UserName)
            .ToListAsync(cancellationToken);
    }

    public Task<AppUser?> FindAsync(string userName, CancellationToken cancellationToken)
    {
        string key = userName.Trim().ToLower();
        return _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.UserName.ToLower() == key, cancellationToken);
    }

    public bool Verify(AppUser user, string password)
    {
        return _hasher.VerifyHashedPassword(user, user.PasswordHash, password)
            != PasswordVerificationResult.Failed;
    }

    public async Task<UserWriteResult> TryAddReaderAsync(
        string userName,
        string password,
        CancellationToken cancellationToken)
    {
        UserWriteResult result = new UserWriteResult();
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            result.Error = "User name and password are required.";
            return result;
        }

        if (await FindAsync(userName, cancellationToken) != null)
        {
            result.Error = "That user name is already taken.";
            return result;
        }

        AppUser user = new AppUser
        {
            UserName = userName.Trim(),
            Role = AppRoles.Reader
        };
        user.PasswordHash = _hasher.HashPassword(user, password);
        user.Scopes.Add(AppScopes.PostsRead);
        _db.Users.Add(user);
        result.User = user;
        return result;
    }
}
