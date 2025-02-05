using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Infrastructure.Conventions;
using WebStore9.Infrastructure.Middleware;
using WebStore9.Services;
using WebStore9.Services.Interfaces;

namespace WebStore9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<WebStore9DB>(opt => 
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            builder.Services.AddSingleton<IProductData, InMemoryProductData>();

            builder.Services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention()))
                .AddRazorRuntimeCompilation();

            var app = builder.Build();

            app.UseStatusCodePagesWithRedirects("~/Home/Status/{0}");

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
