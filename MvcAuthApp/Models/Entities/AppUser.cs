namespace MvcAuthApp.Models;

public class AppUser
{
    public string UserName { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "";
    public List<string> Scopes { get; set; } = new List<string>();
}
