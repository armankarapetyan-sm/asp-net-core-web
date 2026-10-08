using Microsoft.EntityFrameworkCore;
using MvcAuthApp.Data.Repositories;

namespace MvcAuthApp.Data;

public static class DataServices
{
    public static void AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(ResolveConnection(configuration));
        });
        services.AddScoped<IUnitOfWork>(scope => scope.GetRequiredService<AppDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IWallRepository, WallRepository>();
    }

    public static void ApplyDatabase(this IHost app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        DbSeed.Run(db);
    }

    public static string ResolveConnection(IConfiguration configuration)
    {
        string? connection = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connection))
        {
            return "Data Source=app.db";
        }

        return connection;
    }
}
