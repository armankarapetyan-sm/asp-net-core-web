using Microsoft.AspNetCore.Identity;
using MvcAuthApp.Auth;
using MvcAuthApp.Models;

namespace MvcAuthApp.Data;

public static class DbSeed
{
    public static void Run(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();
            foreach (DemoPerson person in DemoPeople.Seed)
            {
                AppUser user = new AppUser
                {
                    UserName = person.UserName,
                    Role = person.Role
                };
                user.PasswordHash = hasher.HashPassword(user, person.Password);
                foreach (string scope in person.Scopes)
                {
                    user.Scopes.Add(scope);
                }

                db.Users.Add(user);
            }
        }

        if (!db.Posts.Any())
        {
            db.Posts.Add(new Post
            {
                Title = "Welcome",
                Content = "Only people with posts.read can see this list.",
                Author = "admin"
            });
            db.Posts.Add(new Post
            {
                Title = "Editor note",
                Content = "The author can edit this row. A reader cannot.",
                Author = "editor"
            });
        }

        if (!db.WallNotes.Any())
        {
            db.WallNotes.Add(new WallNote
            {
                Author = "admin",
                Body = "Hello. This line is plain text."
            });
        }

        db.SaveChanges();
    }
}
