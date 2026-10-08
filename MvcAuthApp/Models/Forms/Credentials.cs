namespace MvcAuthApp.Models;

/// <summary>Mock user name and password (same as the HTML login).</summary>
public class Credentials
{
    /// <summary>Example: reader</summary>
    public string UserName { get; set; } = "";

    /// <summary>Example: reader123</summary>
    public string Password { get; set; } = "";
}
