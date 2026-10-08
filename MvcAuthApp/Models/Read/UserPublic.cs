namespace MvcAuthApp.Models;

/// <summary>A user without the password hash.</summary>
public class UserPublic
{
    public string UserName { get; set; } = "";
    public string Role { get; set; } = "";
    public List<string> Scopes { get; set; } = new List<string>();

    public static UserPublic From(AppUser user)
    {
        UserPublic row = new UserPublic
        {
            UserName = user.UserName,
            Role = user.Role
        };
        foreach (string scope in user.Scopes)
        {
            row.Scopes.Add(scope);
        }

        return row;
    }

    public static List<UserPublic> Many(IEnumerable<AppUser> users)
    {
        List<UserPublic> rows = new List<UserPublic>();
        foreach (AppUser user in users)
        {
            rows.Add(From(user));
        }

        return rows;
    }
}
