namespace MvcAuthApp.Models;

/// <summary>Who the current proof says you are.</summary>
public class MeResponse
{
    public string Name { get; set; } = "";
    public string Role { get; set; } = "";
    public string Proof { get; set; } = "";
    public List<string> Scopes { get; set; } = new List<string>();
}
