using MvcAuthApp.Models;

namespace MvcAuthApp.Data;

public class UserWriteResult
{
    public AppUser? User { get; set; }
    public string Error { get; set; } = "";
}
