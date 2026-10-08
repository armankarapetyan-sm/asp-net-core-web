namespace MvcAuthApp.Models;

public class WallNoteView
{
    public string Author { get; set; } = "";
    public string Body { get; set; } = "";
    public bool ProtectedSave { get; set; }

    public static WallNoteView From(WallNote note)
    {
        return new WallNoteView
        {
            Author = note.Author,
            Body = note.Body,
            ProtectedSave = note.ProtectedSave
        };
    }

    public static List<WallNoteView> Many(IEnumerable<WallNote> notes)
    {
        List<WallNoteView> rows = new List<WallNoteView>();
        foreach (WallNote note in notes)
        {
            rows.Add(From(note));
        }

        return rows;
    }
}
