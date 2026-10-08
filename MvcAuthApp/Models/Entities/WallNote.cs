namespace MvcAuthApp.Models;

public class WallNote
{
    public int Id { get; set; }
    public string Author { get; set; } = "";
    public string Body { get; set; } = "";
    public bool ProtectedSave { get; set; }
}
