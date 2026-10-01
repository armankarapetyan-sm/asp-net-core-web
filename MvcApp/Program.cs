namespace MvcApp;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();

        WebApplication app = builder.Build();
        // middleware — runs before the controller
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Post}/{action=Index}/{id?}");
        app.Run();
    }
}
