using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using WebStore9.Infrastructure.Conventions;
using WebStore9.Infrastructure.Middleware;
using WebStore9.Interfaces.Services;
using WebStore9.Interfaces.TestAPI;
using WebStore9.Logger;
using WebStore9.Services.Services.InCookies;
using WebStore9.WebAPI.Clients.Employees;
using WebStore9.WebAPI.Clients.Identity;
using WebStore9.WebAPI.Clients.Orders;
using WebStore9.WebAPI.Clients.Products;
using WebStore9.WebAPI.Clients.Values;
using WebStore9Domain.Entities.Identity;

namespace WebStore9
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddIdentity<User, Role>()
                .AddIdentityWebStoreWebAPIClients()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore9";
                opt.Cookie.HttpOnly = true;

                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ICartService, InCookiesCartService>();

            builder.Services.AddHttpClient("WebStore9WabAPI", client => client.BaseAddress = new(builder.Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IEmployeesData, EmployeesClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrderService, OrdersClient>();

            builder.Services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention()))
                .AddRazorRuntimeCompilation();

            //builder.Host.ConfigureLogging((host, log) => log
            //    .ClearProviders()
            //    .AddConsole(c => c.IncludeScopes = true)
            //    .AddFilter("Microsoft", LogLevel.Warning)
            //);
            builder.Host.UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile($@".\Logs\WebStore9[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
                .WriteTo.File(new JsonFormatter(",",true), $@".\Logs\WebStore9[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
            );

            var app = builder.Build();

            app.Services.GetRequiredService<ILoggerFactory>().AddLog4Net();

            app.UseStatusCodePagesWithRedirects("~/Home/Status/{0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<TestMiddleware>();

            app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapGet("/greetings", () => app.Configuration["Greetings"]);

            await app.RunAsync();
        }
    }
}
