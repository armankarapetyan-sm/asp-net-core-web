using Microsoft.EntityFrameworkCore;
using MvcAuthApp.Models;

namespace MvcAuthApp.Data.Repositories;

public class WallRepository : IWallRepository
{
    private readonly AppDbContext _db;

    public WallRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<WallNote>> ListAsync(CancellationToken cancellationToken)
    {
        return await _db.WallNotes
            .AsNoTracking()
            .OrderBy(note => note.Id)
            .ToListAsync(cancellationToken);
    }

    public void Add(WallNote note)
    {
        _db.WallNotes.Add(note);
    }

    public void Clear()
    {
        _db.WallNotes.RemoveRange(_db.WallNotes);
    }
}
