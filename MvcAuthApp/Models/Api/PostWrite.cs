namespace MvcAuthApp.Models;

/// <summary>Create or update. Author is set on the server.</summary>
public class PostWrite
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
}
