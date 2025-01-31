using WebStore9.Infrastructure.Middleware;

namespace WebStore9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<TestMiddleware>();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapGet("/greetings", () => app.Configuration["Greetings"]);

            app.Run();
        }
    }
}
