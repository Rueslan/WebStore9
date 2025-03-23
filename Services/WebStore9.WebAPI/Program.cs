using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using WebStore9.DAL.Context;
using WebStore9.Infrastructure.Middleware;
using WebStore9.Interfaces.Services;
using WebStore9.Logger;
using WebStore9.Services.Data;
using WebStore9.Services.Services;
using WebStore9.Services.Services.InCookies;
using WebStore9.Services.Services.InMemory;
using WebStore9.Services.Services.InSQL;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var databasetype = builder.Configuration["Database"];

            switch (databasetype)
            {
                default: throw new InvalidOperationException($"Тип БД {databasetype} не поддерживается");

                case "SqlServer":
                    builder.Services.AddDbContext<WebStore9DB>(opt =>
                        opt.UseSqlServer(builder.Configuration.GetConnectionString(databasetype)));
                    break;

                case "Sqlite":
                    SQLitePCL.Batteries.Init();
                    builder.Services.AddDbContext<WebStore9DB>(opt =>
                        opt.UseSqlite(builder.Configuration.GetConnectionString(databasetype),
                            o => o.MigrationsAssembly("WebStore9.DAL.Sqlite")));
                    break;

                case "InMemory":
                    builder.Services.AddDbContext<WebStore9DB>(opt =>
                        opt.UseInMemoryDatabase("WebStore9.db"));
                    break;
            }

            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStore9DB>()
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

            builder.Services.AddScoped<WebStore9DBInitializer>();

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            builder.Services.AddScoped<IProductData, SqlProductData>();
            builder.Services.AddScoped<ICartStore, InCookiesCartStore>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, SqlOrderService>();

            builder.Services.AddControllers();

            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(options =>
                {
                    options.EnableAnnotations();
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore9 API", Version = "v1" });

                    const string webstoreApiXml = "WebStore9.WebAPI.xml";
                    const string webstoreDomainXml = "WebStore9.Domain.xml";
                    const string debugPath = "bin/debug/net9.0";

                    if (File.Exists(webstoreApiXml))
                        options.IncludeXmlComments(webstoreApiXml);
                    else if (File.Exists(Path.Combine(debugPath, webstoreApiXml)))
                        options.IncludeXmlComments(Path.Combine(debugPath, webstoreApiXml));

                    if (File.Exists(webstoreDomainXml))
                        options.IncludeXmlComments(webstoreDomainXml);
                    else if (File.Exists(Path.Combine(debugPath, webstoreDomainXml)))
                        options.IncludeXmlComments(Path.Combine(debugPath, webstoreDomainXml));
                });

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            //app.MapHealthChecks("/health");

            app.Services.GetRequiredService<ILoggerFactory>().AddLog4Net();

            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<WebStore9DBInitializer>();
                await initializer.InitializeAsync();
            }


            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger().UseSwaggerUI();
            }

            app.MapGet("/", () => Results.LocalRedirect("/swagger")).ExcludeFromDescription();


            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
