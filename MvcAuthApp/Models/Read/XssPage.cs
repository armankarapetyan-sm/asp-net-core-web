namespace MvcAuthApp.Models;

public class XssPage
{
    public IReadOnlyList<WallNoteView> Notes { get; set; } = new List<WallNoteView>();
    public WallNoteForm Form { get; set; } = new WallNoteForm();
}
