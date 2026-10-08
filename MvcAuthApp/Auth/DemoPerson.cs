namespace MvcAuthApp.Auth;

public class DemoPerson
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
    public string[] Scopes { get; set; } = Array.Empty<string>();
}

public static class DemoPeople
{
    public static readonly DemoPerson[] Seed =
    {
        new DemoPerson
        {
            UserName = "admin",
            Password = "admin123",
            Role = AppRoles.Admin,
            Scopes = new[] { AppScopes.PostsRead, AppScopes.PostsWrite, AppScopes.UsersManage }
        },
        new DemoPerson
        {
            UserName = "editor",
            Password = "editor123",
            Role = AppRoles.Editor,
            Scopes = new[] { AppScopes.PostsRead, AppScopes.PostsWrite }
        },
        new DemoPerson
        {
            UserName = "reader",
            Password = "reader123",
            Role = AppRoles.Reader,
            Scopes = new[] { AppScopes.PostsRead }
        },
        new DemoPerson
        {
            UserName = "guest",
            Password = "guest123",
            Role = AppRoles.Guest
        }
    };
}
