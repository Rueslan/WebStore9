using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Data;
using WebStore9.Infrastructure.Conventions;
using WebStore9.Infrastructure.Middleware;
using WebStore9.Services.InMemory;
using WebStore9.Services.InSQL;
using WebStore9.Services.Interfaces;

namespace WebStore9
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<WebStore9DB>(opt => 
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddTransient<WebStore9DBInitializer>();

            builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            //builder.Services.AddSingleton<IProductData, InMemoryProductData>();
            builder.Services.AddScoped<IProductData, SqlProductData>();

            builder.Services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention()))
                .AddRazorRuntimeCompilation();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<WebStore9DBInitializer>();
                await initializer.InitializeAsync();
            }

            app.UseStatusCodePagesWithRedirects("~/Home/Status/{0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<TestMiddleware>();

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapGet("/greetings", () => app.Configuration["Greetings"]);

            await app.RunAsync();
        }
    }
}
