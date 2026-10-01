using ApiHost.Data;

namespace ApiHost;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<PostStore>();

        WebApplication app = builder.Build();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        app.MapControllerRoute(
            name: "pages",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
}
