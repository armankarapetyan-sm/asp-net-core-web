using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcAuthApp.Models;

namespace MvcAuthApp.Data;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<WallNote> WallNotes => Set<WallNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ValueConverter<List<string>, string> scopes = new ValueConverter<List<string>, string>(
            list => string.Join(",", list),
            text => SplitScopes(text));
        ValueComparer<List<string>> scopesSame = new ValueComparer<List<string>>(
            (left, right) => ScopesEqual(left, right),
            list => ScopesHash(list),
            list => new List<string>(list));

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.UserName);
            entity.Property(user => user.UserName).HasMaxLength(64);
            entity.Property(user => user.PasswordHash).HasMaxLength(256);
            entity.Property(user => user.Role).HasMaxLength(32);
            entity.Property(user => user.Scopes)
                .HasConversion(scopes, scopesSame)
                .HasMaxLength(256);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.Property(post => post.Title).HasMaxLength(128);
            entity.Property(post => post.Author).HasMaxLength(64);
        });

        modelBuilder.Entity<WallNote>(entity =>
        {
            entity.ToTable("WallNotes");
            entity.Property(note => note.Author).HasMaxLength(64);
        });
    }

    private static List<string> SplitScopes(string text)
    {
        List<string> scopes = new List<string>();
        if (string.IsNullOrWhiteSpace(text))
        {
            return scopes;
        }

        foreach (string part in text.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            scopes.Add(part);
        }

        return scopes;
    }

    private static bool ScopesEqual(List<string>? left, List<string>? right)
    {
        if (left == null || right == null)
        {
            return left == right;
        }

        if (left.Count != right.Count)
        {
            return false;
        }

        for (int i = 0; i < left.Count; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }

        return true;
    }

    private static int ScopesHash(List<string>? list)
    {
        if (list == null)
        {
            return 0;
        }

        HashCode hash = new HashCode();
        foreach (string scope in list)
        {
            hash.Add(scope);
        }

        return hash.ToHashCode();
    }
}
