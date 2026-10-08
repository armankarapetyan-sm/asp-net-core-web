namespace MvcAuthApp.Models;

public class XssLabPage
{
    public IReadOnlyList<WallNoteView> Comments { get; set; } = new List<WallNoteView>();
    public IReadOnlyList<XssLabAttack> Attacks { get; set; } = new List<XssLabAttack>();
    public bool Cleared { get; set; }
}
