namespace MvcAuthApp.Models;

/// <summary>Returned when the Admin role check passed.</summary>
public class RolesCheckResponse
{
    public bool Ok { get; set; }
    public string Name { get; set; } = "";
}
